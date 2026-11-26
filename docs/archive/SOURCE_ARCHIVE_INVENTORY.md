# جرد مصادر الأرشيفات — 2026-11-26

**المستودع المرجعي:** `ar-and-you/CostOfStaying`  
**حالة المواد:** تاريخية وغير نافذة؛ لا تتجاوز الملفات والسياسات الحية في `main`.  
**تقرير التحقيق:** [مصالحة الأرشيفات وخطة التحقيق](../operations/ARCHIVE_RECONCILIATION_2026-11-26.md).

## 1. الأرشيفات السبعة المرفوعة مباشرة

| الأرشيف | الحجم بالبايت | SHA-256 | إدخالات ZIP |
|---|---:|---|---:|
| `CostOfStaying-FINAL-verified-2026-11-20.zip` | 6852700 | `628a351cc21989c1b6a0152b61bfa5e9d0e1b6d25b42420dcce7ab016a6b0946` | 408 |
| `CostOfStaying_MASTER_UNIFIED_2026-11-19.zip` | 3064360 | `116c9e797aa6f2527eb79aa939b1a5a5366386ed87bda5c2a23662eb16fb5f29` | 270 |
| `CostOfStaying.zip` | 7295275 | `ad2855693b013cd2c11fa68b319a334d88896fd0c49806d7a41f89daaec09937` | 11 |
| `ملفات لعبة .zip` | 19891288 | `e0a972b42f68f4c6dd23f6400e45f69e53c026850d2e85d6be2b2cf29f5094f6` | 7 |
| `CostOfStaying-FINAL-upload-package.zip` | 6126779 | `6369be0250b0e7ba3da6defe1ca9eb3ec06d79e5c1e7d43515e81a960b3f24cf` | 5 |
| `تحليل.zip` | 3070217 | `5228c6b91e06112d437a550b904738575b6f162a12ed02ef05a3f24c3e33a5ed` | 8 |
| `CostOfStaying_AI_Documentation_Toolkit.zip` | 26122 | `43b3bf24013c069f5de31cc1b95db6517f8731fb910be15614ed5c1fd0a1e967` | 24 |

## 2. الأرشيفات الداخلية الفريدة

| الأرشيف الداخلي | الحجم بالبايت | SHA-256 | إدخالات |
|---|---:|---|---:|
| `CostOfStaying-DOCUMENTATION-CANONICAL-2026-11-23.zip` | 3016034 | `778e8d2cb59347c2f6464f701cfc05f9cb70a780ade0c3d64a5d4109277396ac` | 198 |
| `CostOfStaying-PHASE1-DOCUMENTATION-GOVERNANCE-CLOSURE-2026-11-23.zip` | 68073 | `8b6d0ba5c2e5b3ecfa8734f429ef9aee4aeb751be3f7d745462e98fe81ecf6e8` | 40 |
| `CostOfStaying_GOVERNANCE_FINAL_CORRECTIONS_2026-11-22.zip` | 8103 | `c41b7259dbaa7db96aa3591dc45c98237dbf99730f0cfb5f335fd40f0d6cc569` | 9 |
| `CostOfStaying_Local_Completion_Package.zip` | 22017 | `58240ad30daac4f7293b390b8ad5b85b047e2f2cdda050c8a27078fa3d3dda71` | 28 |
| `CostOfStaying_SECURITY_DOCS_RECONCILIATION_2026-11-22.zip` | 12512 | `77b646612875276b7e6505567be21290b2a96af5836715c4ae004b691eed8971` | 8 |
| `CostOfStaying-FINAL.zip` | 6807926 | `d68f7737027560aa9396b55b2a0c5eaac14f6ccdc7c732bd6319f5c13b0b33b7` | 373 |
| `CostOfStaying-baseline-update.zip` | 453324 | `522292c0203850ad2bfce30bcf5114000aa9571abe1e785ab9ac9cf717c88f13` | 289 |
| `CostOfStaying-final 1.zip` | 862923 | `1fcb849c9164ac233f877a9eba58ceea8d7854feb55053e9fa70f57024b9642c` | 317 |
| `CostOfStaying-final.zip` | 813438 | `25c8e6b7ff0910dccccbbc30d2870ba8b2eae63268bd5b464eab2dfa482db286` | 313 |
| `CostOfStaying-v3.0-clean(1).zip` | 197905 | `efa82caad905f68049d3a2797e73dfba132fc4083bd2c2c79b2e89578a307edc` | 139 |
| `CostOfStaying_FULL_BACKUP.zip` | 4221764 | `b947fc449a1208b03ca4d62c0521864e7315e0efd5bf11cb95fd911e414f0bf2` | 198 |
| `CostOfStaying-main-8b811b0.zip` | 3072377 | `1e0d2a4e9140d4942b32ed32041c2c90d6c7de4b9e859a988ad5c8e258954f35` | 309 |

توجد نسخ متكررة من بعض الأرشيفات الداخلية، ومنها أرشيف FINAL-verified نفسه داخل أكثر من حاوية؛ احتُسب المحتوى مرة واحدة عند حساب الأرشيفات الفريدة.

## 3. نتائج التحقق

- جميع ملفات ZIP الـ21 التي ظهرت عند فك التداخل اجتازت `testzip()`؛ لا توجد أخطاء CRC مسجلة.
- 19 محتوى ZIP فريداً بعد إزالة النسخ المتطابقة ببصمة الأرشيف.
- 2,535 إدخال ملف عبر الأرشيفات الفريدة؛ 788 بصمة SHA-256 فريدة، و368 مجموعة تطابق محتوى عبر مسارات/حاويات مختلفة.
- Toolkit: 23/23 بصمة داخلية مطابقة.
- Upload package: 4/4 بصمات داخلية مطابقة.
- Git Bundle في حزمة الرفع صالح بنيوياً ويحتوي تاريخاً كاملاً عند `8b811b0c23329481dd49de0586616f75558a1f89`، لكن GitHub أعاد `422 No commit found for SHA` عند طلب ذلك الالتزام من المستودع الرسمي. لا تستخدمه كـmain ولا تدفعه.

## 4. حدود الجرد

سلامة ZIP وتطابق SHA لا يثبتان صحة المحتوى أو حداثته أو قبوله. المقارنة الدلالية والملفية الشاملة مع `main` ما زالت قيد التنفيذ. لا تستورد ملفات أو ADRs أو workflows من هذه الحزم لمجرد وجودها فيها.

الأرشيفات الأصلية تبقى محفوظة لدى المالك؛ لا تُحذف بعد تسجيل هذا الجرد. راجع تقرير المصالحة لمعرفة ما أُنجز وما لا يزال مفتوحاً.
