#!/usr/bin/env python3
"""
Markdown → HTML 一括変換スクリプト
全.mdファイルをindex.htmlと同じデザインテーマのHTMLに変換する
"""

import os
import glob
import markdown
from pathlib import Path

# プロジェクトルート
ROOT_DIR = Path(__file__).parent.parent

# HTMLテンプレート
HTML_TEMPLATE = """<!DOCTYPE html>
<html lang="ja">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>{title} - BSE訓練教材</title>
<style>
:root {{
    --primary: #005BAB;
    --primary-light: #e8f0fe;
    --primary-dark: #004080;
    --text: #1a1a1a;
    --text-light: #555;
    --bg: #f8f9fa;
    --card-bg: #fff;
    --border: #e0e0e0;
    --code-bg: #f5f5f5;
    --success: #28a745;
    --warning: #ffc107;
    --danger: #dc3545;
}}

* {{ margin: 0; padding: 0; box-sizing: border-box; }}

body {{
    font-family: system-ui, -apple-system, sans-serif;
    color: var(--text);
    background: var(--bg);
    line-height: 1.8;
}}

.header {{
    background: linear-gradient(135deg, var(--primary), var(--primary-dark));
    color: #fff;
    padding: 20px 30px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}}

.header h1 {{
    font-size: 1.2rem;
    font-weight: 600;
}}

.header a {{
    color: #fff;
    text-decoration: none;
    opacity: 0.85;
    font-size: 0.9rem;
}}

.header a:hover {{
    opacity: 1;
    text-decoration: underline;
}}

.breadcrumb {{
    background: var(--card-bg);
    padding: 10px 30px;
    border-bottom: 1px solid var(--border);
    font-size: 0.85rem;
    color: var(--text-light);
}}

.breadcrumb a {{
    color: var(--primary);
    text-decoration: none;
}}

.breadcrumb a:hover {{
    text-decoration: underline;
}}

.content {{
    max-width: 900px;
    margin: 30px auto;
    padding: 40px;
    background: var(--card-bg);
    border-radius: 8px;
    box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}}

h1 {{
    font-size: 1.8rem;
    color: var(--primary);
    border-bottom: 3px solid var(--primary);
    padding-bottom: 10px;
    margin-bottom: 25px;
}}

h2 {{
    font-size: 1.4rem;
    color: var(--primary-dark);
    margin-top: 35px;
    margin-bottom: 15px;
    padding-bottom: 8px;
    border-bottom: 2px solid var(--primary-light);
}}

h3 {{
    font-size: 1.15rem;
    color: var(--text);
    margin-top: 25px;
    margin-bottom: 10px;
}}

h4 {{
    font-size: 1.05rem;
    color: var(--text-light);
    margin-top: 20px;
    margin-bottom: 8px;
}}

p {{
    margin-bottom: 12px;
}}

ul, ol {{
    margin-bottom: 15px;
    padding-left: 25px;
}}

li {{
    margin-bottom: 5px;
}}

table {{
    width: 100%;
    border-collapse: collapse;
    margin: 15px 0;
    font-size: 0.92rem;
}}

th {{
    background: var(--primary);
    color: #fff;
    padding: 10px 12px;
    text-align: left;
    font-weight: 600;
}}

td {{
    padding: 8px 12px;
    border-bottom: 1px solid var(--border);
}}

tr:nth-child(even) {{
    background: var(--primary-light);
}}

tr:hover {{
    background: #dbeafe;
}}

code {{
    background: var(--code-bg);
    padding: 2px 6px;
    border-radius: 3px;
    font-family: 'Consolas', 'Monaco', monospace;
    font-size: 0.88rem;
    color: #c7254e;
}}

pre {{
    background: #1e1e1e;
    color: #d4d4d4;
    padding: 20px;
    border-radius: 6px;
    overflow-x: auto;
    margin: 15px 0;
    line-height: 1.5;
}}

pre code {{
    background: none;
    color: inherit;
    padding: 0;
    font-size: 0.85rem;
}}

blockquote {{
    border-left: 4px solid var(--primary);
    background: var(--primary-light);
    padding: 12px 20px;
    margin: 15px 0;
    border-radius: 0 6px 6px 0;
}}

blockquote p {{
    margin-bottom: 0;
}}

strong {{
    color: var(--primary-dark);
}}

hr {{
    border: none;
    border-top: 2px solid var(--border);
    margin: 30px 0;
}}

a {{
    color: var(--primary);
}}

a:hover {{
    color: var(--primary-dark);
}}

.nav-links {{
    display: flex;
    justify-content: space-between;
    margin-top: 40px;
    padding-top: 20px;
    border-top: 2px solid var(--border);
}}

.nav-links a {{
    display: inline-block;
    padding: 8px 16px;
    background: var(--primary);
    color: #fff;
    text-decoration: none;
    border-radius: 4px;
    font-size: 0.9rem;
}}

.nav-links a:hover {{
    background: var(--primary-dark);
}}

/* === 図解用スタイル === */

/* バグサマリーカード */
.bug-summary {{
    display: flex;
    gap: 12px;
    margin: 25px 0;
    flex-wrap: wrap;
}}
.bug-card {{
    flex: 1;
    min-width: 200px;
    padding: 15px;
    border-radius: 8px;
    background: #fff;
    border-left: 5px solid;
    box-shadow: 0 2px 4px rgba(0,0,0,0.08);
}}
.bug-card.high {{ border-color: var(--danger); }}
.bug-card.medium {{ border-color: #fd7e14; }}
.bug-card .bug-no {{
    font-size: 0.75rem;
    color: #888;
    margin-bottom: 4px;
}}
.bug-card .bug-title {{
    font-weight: bold;
    color: var(--text);
    margin-bottom: 6px;
    font-size: 0.95rem;
}}
.bug-card .bug-severity {{
    display: inline-block;
    padding: 2px 10px;
    border-radius: 4px;
    color: #fff;
    font-size: 0.72rem;
    font-weight: bold;
}}
.bug-card.high .bug-severity {{ background: var(--danger); }}
.bug-card.medium .bug-severity {{ background: #fd7e14; }}

/* フローチャート */
.flow-chart {{
    text-align: center;
    margin: 25px auto;
    padding: 20px;
    background: #f8f9fa;
    border-radius: 8px;
    border: 1px solid var(--border);
}}
.flow-step {{
    display: inline-block;
    padding: 10px 24px;
    border-radius: 6px;
    margin: 4px 0;
    font-size: 0.88rem;
    max-width: 90%;
}}
.flow-step.normal {{
    background: var(--primary);
    color: #fff;
}}
.flow-step.bug {{
    background: var(--danger);
    color: #fff;
    border: 3px dashed #fff;
    box-shadow: 0 0 0 3px var(--danger);
}}
.flow-step.check {{
    background: var(--primary-light);
    color: var(--primary-dark);
    border: 2px solid var(--primary);
}}
.flow-step.success {{
    background: var(--success);
    color: #fff;
}}
.flow-step.missing {{
    background: #fff;
    color: var(--danger);
    border: 2px dashed var(--danger);
}}
.flow-arrow {{
    display: block;
    font-size: 1.2rem;
    color: var(--primary);
    margin: 2px 0;
    line-height: 1;
}}

/* Before/After比較 */
.comparison {{
    display: flex;
    gap: 15px;
    margin: 20px 0;
}}
.comparison > div {{
    flex: 1;
    padding: 15px;
    border-radius: 8px;
    overflow-x: auto;
}}
.comparison .ng {{
    background: #fff5f5;
    border: 2px solid var(--danger);
}}
.comparison .ok {{
    background: #f0fff4;
    border: 2px solid var(--success);
}}
.comparison .label {{
    font-weight: bold;
    margin-bottom: 8px;
    font-size: 0.9rem;
}}
.comparison .ng .label {{ color: var(--danger); }}
.comparison .ok .label {{ color: var(--success); }}
.comparison code {{
    display: block;
    white-space: pre;
    font-size: 0.82rem;
    line-height: 1.5;
    background: none;
    padding: 0;
    color: inherit;
}}

/* 概念図ボックス */
.concept-box {{
    margin: 20px 0;
    padding: 20px;
    background: #f8f9fa;
    border-radius: 8px;
    border: 2px solid var(--border);
}}
.concept-title {{
    font-weight: bold;
    color: var(--primary);
    margin-bottom: 12px;
    font-size: 1rem;
}}

/* 計算可視化 */
.calc-visual {{
    display: flex;
    align-items: center;
    gap: 10px;
    justify-content: center;
    margin: 15px 0;
    flex-wrap: wrap;
}}
.calc-box {{
    padding: 8px 16px;
    border-radius: 6px;
    font-weight: bold;
    font-size: 0.95rem;
    text-align: center;
}}
.calc-box.value {{ background: var(--primary-light); color: var(--primary-dark); }}
.calc-box.op {{ background: transparent; color: var(--text-light); font-size: 1.1rem; }}
.calc-box.result-ng {{ background: #fff5f5; color: var(--danger); border: 2px solid var(--danger); }}
.calc-box.result-ok {{ background: #f0fff4; color: var(--success); border: 2px solid var(--success); }}

/* ステップ図（ガイド用） */
.step-visual {{
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
    margin: 20px 0;
    justify-content: center;
    align-items: center;
}}
.step-num {{
    width: 36px;
    height: 36px;
    border-radius: 50%;
    background: var(--primary);
    color: #fff;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: 0.9rem;
    flex-shrink: 0;
}}
.step-label {{
    font-size: 0.88rem;
    color: var(--text);
    padding: 6px 12px;
    background: var(--primary-light);
    border-radius: 4px;
}}
.step-connector {{
    color: var(--primary);
    font-size: 1.2rem;
}}

/* インラインSVG調整 */
.content svg {{
    max-width: 100%;
    height: auto;
    display: block;
    margin: 15px auto;
}}

/* 影響度インジケーター */
.impact-row {{
    display: flex;
    align-items: center;
    gap: 8px;
    margin: 8px 0;
    padding: 8px 12px;
    border-radius: 4px;
    background: #f8f9fa;
}}
.impact-dot {{
    width: 14px;
    height: 14px;
    border-radius: 50%;
    flex-shrink: 0;
}}
.impact-dot.high {{ background: var(--danger); }}
.impact-dot.medium {{ background: #fd7e14; }}

/* データフロー図 */
.data-flow {{
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 12px;
    margin: 20px 0;
    flex-wrap: wrap;
}}
.data-node {{
    padding: 10px 18px;
    border-radius: 6px;
    text-align: center;
    font-size: 0.88rem;
}}
.data-node.source {{ background: var(--primary-light); border: 2px solid var(--primary); color: var(--primary-dark); }}
.data-node.process {{ background: #fff3cd; border: 2px solid #ffc107; color: #856404; }}
.data-node.result {{ background: #d4edda; border: 2px solid var(--success); color: #155724; }}
.data-node.error {{ background: #f8d7da; border: 2px solid var(--danger); color: #721c24; }}
.data-arrow {{
    font-size: 1.3rem;
    color: var(--primary);
}}

@media (max-width: 768px) {{
    .content {{
        margin: 15px;
        padding: 20px;
    }}
    h1 {{ font-size: 1.4rem; }}
    h2 {{ font-size: 1.2rem; }}
    pre {{ padding: 12px; font-size: 0.8rem; }}
    table {{ font-size: 0.82rem; }}
    .comparison {{ flex-direction: column; }}
    .bug-summary {{ flex-direction: column; }}
    .calc-visual {{ flex-direction: column; }}
    .data-flow {{ flex-direction: column; }}
    .step-visual {{ flex-direction: column; align-items: flex-start; }}
}}
</style>
</head>
<body>
<div class="header">
    <h1>BSE コードレビュー訓練教材</h1>
    <a href="{portal_link}">ポータルに戻る</a>
</div>
<div class="breadcrumb">
    <a href="{portal_link}">ポータル</a> / {breadcrumb}
</div>
<div class="content">
{body}
<div class="nav-links">
    <a href="{portal_link}">ポータルに戻る</a>
</div>
</div>
</body>
</html>"""


def get_title_from_md(content: str, filename: str) -> str:
    """Markdownの最初のh1からタイトルを取得"""
    for line in content.split('\n'):
        if line.startswith('# '):
            return line[2:].strip()
    return Path(filename).stem


def get_breadcrumb(rel_path: str) -> str:
    """パスからパンくずリストを生成"""
    parts = Path(rel_path).parts
    if len(parts) <= 1:
        return Path(rel_path).stem
    return ' / '.join(parts[:-1]) + ' / ' + Path(parts[-1]).stem


def get_portal_link(rel_path: str) -> str:
    """相対パスからポータルへの相対リンクを算出"""
    depth = len(Path(rel_path).parts) - 1
    if depth <= 0:
        return 'index.html'
    return '../' * depth + 'index.html'


def convert_md_to_html(md_path: Path, root: Path):
    """1つのmdファイルをHTMLに変換"""
    rel_path = md_path.relative_to(root)
    html_path = md_path.with_suffix('.html')

    with open(md_path, 'r', encoding='utf-8') as f:
        md_content = f.read()

    title = get_title_from_md(md_content, str(md_path))
    breadcrumb = get_breadcrumb(str(rel_path))
    portal_link = get_portal_link(str(rel_path))

    # Markdown → HTML変換
    extensions = ['tables', 'fenced_code', 'codehilite', 'nl2br', 'sane_lists']
    html_body = markdown.markdown(md_content, extensions=extensions)

    # テンプレートに埋め込み
    full_html = HTML_TEMPLATE.format(
        title=title,
        body=html_body,
        breadcrumb=breadcrumb,
        portal_link=portal_link
    )

    with open(html_path, 'w', encoding='utf-8') as f:
        f.write(full_html)

    return str(rel_path), str(html_path.relative_to(root))


def main():
    # 全.mdファイルを検索
    md_files = sorted(ROOT_DIR.glob('**/*.md'))

    print(f"変換対象: {len(md_files)} ファイル")
    print("=" * 50)

    converted = []
    for md_file in md_files:
        rel, html_rel = convert_md_to_html(md_file, ROOT_DIR)
        converted.append((rel, html_rel))
        print(f"  {rel} -> {html_rel}")

    print("=" * 50)
    print(f"変換完了: {len(converted)} ファイル")


if __name__ == '__main__':
    main()
