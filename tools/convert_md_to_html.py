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

@media (max-width: 768px) {{
    .content {{
        margin: 15px;
        padding: 20px;
    }}
    h1 {{ font-size: 1.4rem; }}
    h2 {{ font-size: 1.2rem; }}
    pre {{ padding: 12px; font-size: 0.8rem; }}
    table {{ font-size: 0.82rem; }}
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
