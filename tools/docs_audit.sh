#!/usr/bin/env bash
set -euo pipefail

python3 - <<'PY'
from pathlib import Path
import re
import sys
from urllib.parse import unquote

root = Path.cwd().resolve()
docs_root = root / "docs"
index = docs_root / "README.md"
if not docs_root.is_dir() or not index.is_file():
    print("ERROR: docs/README.md is required")
    sys.exit(2)

md_files = sorted(docs_root.rglob("*.md"))
known = {p.resolve() for p in md_files}
known.update(p.resolve() for p in root.glob("*.md"))
link_re = re.compile(r"!?\[[^\]]*\]\(([^)]+)\)")
ref_re = re.compile(r"^\s*\[[^\]]+\]:\s*(\S+)", re.MULTILINE)
broken = []
edges = {p.resolve(): set() for p in md_files}

def resolve_target(source: Path, raw: str):
    raw = raw.strip().strip("<>").split()[0] if raw.strip() else ""
    if not raw or raw.startswith("#"):
        return None, None
    if raw.lower().startswith(("https://", "http://", "mailto:", "tel:", "data:", "javascript:")):
        return None, None
    target = unquote(raw.split("#", 1)[0])
    if not target:
        return None, None
    path = (source.parent / target).resolve()
    if path.is_dir():
        return path, "dir"
    if path.is_file():
        return path, "file"
    for candidate in (path, path.with_suffix(".md"), path / "README.md", path / "index.md"):
        if candidate.is_file():
            return candidate.resolve(), "file"
    return path, "missing"

for source in md_files:
    text = source.read_text(encoding="utf-8")
    text = re.sub(r"```[\s\S]*?```", "", text)
    text = re.sub(r"~~~[\s\S]*?~~~", "", text)
    text = re.sub(r"`[^`]*`", "", text)
    links = [m.group(1) for m in link_re.finditer(text)]
    links.extend(m.group(1) for m in ref_re.finditer(text))
    for raw in links:
        target, kind = resolve_target(source, raw)
        if kind == "missing":
            broken.append((source.relative_to(root), raw))
        elif kind == "file" and target in known and target.suffix.lower() == ".md":
            edges[source.resolve()].add(target)
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
    print("BROKEN MARKDOWN LINKS:")
    for source, raw in broken:
        print(f"  {source}: {raw}")
if orphans:
    print("ORPHAN DOCUMENTS (not reachable from docs/README.md):")
    for path in orphans:
        print(f"  {path}")
if broken or orphans:
    print(f"FAILED: {len(broken)} broken links, {len(orphans)} orphan documents")
    sys.exit(1)
print(f"OK: {len(md_files)} Markdown documents; no broken relative links; all reachable from docs/README.md")
PY
