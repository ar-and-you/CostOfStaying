#!/usr/bin/env bash
set -euo pipefail

python3 - <<'PY'
from pathlib import Path
import re
import sys
from urllib.parse import unquote, urlsplit

root = Path.cwd().resolve()
docs_root = root / "docs"
index = docs_root / "README.md"
if not docs_root.is_dir() or not index.is_file():
    print("ERROR: docs/README.md is required")
    sys.exit(2)

md_files = sorted(docs_root.rglob("*.md"))
link_re = re.compile(r"!?\[[^\]]*\]\(([^)]+)\)")
ref_re = re.compile(r"^\s*\[[^\]]+\]:\s*(\S+)", re.MULTILINE)
broken = []
edges = {p.resolve(): set() for p in md_files}

def resolve_target(source: Path, raw: str):
    raw = raw.strip()
    if not raw:
        return None, None
    if raw.startswith("<") and ">" in raw:
        raw = raw[1:raw.index(">")]
    else:
        raw = raw.split()[0]
    parts = urlsplit(raw)
    if parts.scheme.lower() in {"https", "http", "mailto", "tel", "data", "javascript"}:
        return None, None
    if parts.netloc or raw.startswith("#"):
        return None, None
    target_text = unquote(parts.path)
    if not target_text:
        return None, None
    candidate = (source.parent / target_text).resolve()
    try:
        candidate.relative_to(root)
    except ValueError:
        return candidate, "outside"
    if candidate.is_dir():
        return candidate, "dir"
    if candidate.is_file():
        return candidate, "file"
    for option in (candidate, candidate.with_suffix(".md"), candidate / "README.md", candidate / "index.md"):
        if option.is_file():
            try:
                option.resolve().relative_to(root)
            except ValueError:
                return option, "outside"
            return option.resolve(), "file"
    return candidate, "missing"

for source in md_files:
    content = source.read_text(encoding="utf-8")
    content = re.sub(r"```[\s\S]*?```", "", content)
    content = re.sub(r"~~~[\s\S]*?~~~", "", content)
    content = re.sub(r"`[^`]*`", "", content)
    links = [m.group(1) for m in link_re.finditer(content)]
    links.extend(m.group(1) for m in ref_re.finditer(content))
    for raw in links:
        target, kind = resolve_target(source, raw)
        if kind in {"missing", "outside"}:
            broken.append((source.relative_to(root), raw, kind))
        elif kind == "file" and target.suffix.lower() == ".md":
            edges[source.resolve()].add(target.resolve())
        elif kind == "dir":
            for child in md_files:
                if child.resolve().is_relative_to(target) and child != source:
                    edges[source.resolve()].add(child.resolve())

start = index.resolve()
seen = {start}
queue = [start]
while queue:
    current = queue.pop(0)
    for nxt in edges.get(current, set()):
        if nxt not in seen:
            seen.add(nxt)
            queue.append(nxt)

orphans = [p.relative_to(root) for p in md_files if p.resolve() not in seen]
if broken:
    print("BROKEN OR OUT-OF-REPOSITORY MARKDOWN LINKS:")
    for source, raw, kind in broken:
        print(f"  {source}: {raw} ({kind})")
if orphans:
    print("ORPHAN DOCUMENTS (not reachable from docs/README.md):")
    for path in orphans:
        print(f"  {path}")
if broken or orphans:
    print(f"FAILED: {len(broken)} invalid links, {len(orphans)} orphan documents")
    sys.exit(1)
print(f"OK: {len(md_files)} Markdown documents; no broken/outside links; all reachable from docs/README.md")
PY
