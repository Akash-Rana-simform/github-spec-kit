# Quickstart Guide: Bookmark Organizer CLI

**Phase**: 1 (Design & Contracts)  
**Created**: 2026-04-28  
**Purpose**: Quick reference for common workflows and usage patterns

## Installation

### Install via pip (when released)

```bash
pip install bookmark-organizer
```

### Install from source

```bash
git clone https://github.com/your-org/bookmark-organizer.git
cd bookmark-organizer
pip install -e .
```

### Verify installation

```bash
bookmark --version
```

---

## Basic Usage

### Add your first bookmark

```bash
bookmark add https://docs.python.org --title "Python Documentation" --tag python --tag reference
```

**Expected output**:
```
✓ Bookmark added: Python Documentation
  URL: https://docs.python.org
  Tags: python, reference
```

### View all bookmarks

```bash
bookmark list
```

**Expected output**:
```
📚 1 bookmark

Python Documentation [python, reference]
  https://docs.python.org
  Added: 2026-04-28
```

### Search bookmarks

```bash
bookmark search python
```

---

## Common Workflows

### Workflow 1: Quick Bookmark Capture

**Scenario**: You're browsing and want to quickly save interesting links

```bash
# Simple add (title auto-extracted from URL)
bookmark add https://github.com/python/cpython

# Add with tags for organization
bookmark add https://realpython.com/python-click/ --tag python --tag cli --tag tutorial

# Add with notes for context
bookmark add https://example.com --note "Check this out later for project ideas"
```

**Tips**:
- Use short, descriptive tags (e.g., `python`, `tutorial`, `work`)
- Add notes when the URL alone won't remind you why you saved it
- Title is optional - it defaults to the URL if not specified

---

### Workflow 2: Organizing Existing Bookmarks

**Scenario**: You have unsaved browser bookmarks you want to organize

**Step 1**: Export from browser
- Chrome: Bookmarks → Bookmark Manager → ⋮ → Export bookmarks
- Firefox: Bookmarks → Show All Bookmarks → Import and Backup → Export
- Safari: File → Export Bookmarks
- Edge: Favorites → ⋮ → Export favorites

**Step 2**: Import into bookmark organizer
```bash
bookmark import ~/Downloads/bookmarks.html
```

**Step 3**: Review and organize
```bash
# List all bookmarks
bookmark list

# Add tags to uncategorized bookmarks
bookmark edit https://example.com --add-tag web --add-tag reference
```

**Step 4**: Export organized bookmarks back to browser (optional)
```bash
bookmark export ~/Downloads/organized-bookmarks.html
```

---

### Workflow 3: Tag-Based Organization

**Scenario**: You want to organize bookmarks by topic using tags

**Create hierarchical tags**:
```bash
# Programming language tags
bookmark add https://docs.python.org --tag programming/python
bookmark add https://golang.org/doc/ --tag programming/go
bookmark add https://docs.rust-lang.org/ --tag programming/rust

# Project-related tags
bookmark add https://api-docs.example.com --tag work/project-alpha --tag api
bookmark add https://design-system.example.com --tag work/project-alpha --tag design
```

**Find all programming links**:
```bash
bookmark list --tag programming/python
```

**Find project-specific bookmarks**:
```bash
bookmark list --tag work/project-alpha
```

**View all tags**:
```bash
bookmark tags list
```

**Expected output**:
```
📑 Tags (6 total)

api (1)
design (1)
programming/go (1)
programming/python (1)
programming/rust (1)
work/project-alpha (2)
```

---

### Workflow 4: Research & Discovery

**Scenario**: You're researching a topic and want to track relevant resources

**Add bookmarks with descriptive tags**:
```bash
bookmark add https://example.com/article1 --tag research --tag machine-learning --tag paper
bookmark add https://example.com/article2 --tag research --tag machine-learning --tag tutorial
bookmark add https://example.com/dataset --tag research --tag machine-learning --tag dataset
```

**Search within research bookmarks**:
```bash
# Find all ML tutorials
bookmark list --tag machine-learning --tag tutorial

# Find papers on specific topic
bookmark search "neural networks" --tag paper
```

**Export research collection**:
```bash
bookmark export ml-research.html --tag machine-learning
```

---

### Workflow 5: Team Collaboration

**Scenario**: Share curated bookmarks with team members

**Create a shared bookmark list**:
```bash
# Team member 1: Add bookmarks
bookmark add https://docs.project.com --tag team --tag documentation
bookmark add https://status.service.com --tag team --tag monitoring

# Export to share
bookmark export team-bookmarks.json --tag team
```

**Team member 2: Import and add more**:
```bash
# Import team bookmarks
bookmark import team-bookmarks.json

# Add own bookmarks
bookmark add https://internal-wiki.com --tag team --tag wiki

# Export updated list
bookmark export team-bookmarks-updated.json --tag team
```

**Tips**:
- Use consistent tag conventions within the team
- Export to JSON for machine-readable format
- Export to HTML for browser import
- Use Git to track team bookmark file changes

---

### Workflow 6: Cleaning Up Old Bookmarks

**Scenario**: Remove or update outdated bookmarks

**Find old bookmarks**:
```bash
# List by creation date (oldest first)
bookmark list --sort created

# List by last modified
bookmark list --sort modified
```

**Update or remove**:
```bash
# Update title or tags
bookmark edit https://old-url.com --title "Updated Title" --add-tag archive

# Remove obsolete bookmarks
bookmark delete https://dead-link.com --yes
```

**Batch cleanup with tags**:
```bash
# Tag old items for review
bookmark edit https://example.com --add-tag review-later

# List all items needing review
bookmark list --tag review-later

# Remove tag after review
bookmark edit https://example.com --remove-tag review-later
```

---

## Power User Tips

### 1. Shell Aliases

Add to your `.bashrc` or `.zshrc`:

```bash
alias ba='bookmark add'
alias bl='bookmark list'
alias bs='bookmark search'
```

Usage:
```bash
ba https://example.com --tag quick
bl --tag quick
```

### 2. Integration with Other Tools

**Pipe to other commands**:
```bash
# Extract all URLs
bookmark list --json | jq -r '.bookmarks[].url'

# Count bookmarks by tag
bookmark tags list --json | jq '.tags[] | "\(.name): \(.count)"'

# Filter and export
bookmark list --tag python --json | jq '.bookmarks[] | select(.title | contains("tutorial"))'
```

**Combine with curl**:
```bash
# Check if bookmarked links are still alive
for url in $(bookmark list --json | jq -r '.bookmarks[].url'); do
  curl -I "$url" 2>/dev/null | head -n 1
done
```

### 3. Git Integration

Track bookmark changes with Git:

```bash
# Initialize Git repo in bookmark directory
cd ~/.bookmarks
git init
git add bookmarks.json
git commit -m "Initial bookmark collection"

# After adding/editing bookmarks
git add bookmarks.json
git commit -m "Added Python resources"

# View history
git log --oneline

# Diff changes
git diff HEAD~1 bookmarks.json
```

### 4. Backup Strategy

Automated backup script:

```bash
#!/bin/bash
# backup-bookmarks.sh
BACKUP_DIR=~/backups/bookmarks
DATE=$(date +%Y%m%d)
mkdir -p $BACKUP_DIR
cp ~/.bookmarks/bookmarks.json $BACKUP_DIR/bookmarks-$DATE.json
echo "Backed up bookmarks to $BACKUP_DIR/bookmarks-$DATE.json"
```

Add to crontab for daily backups:
```bash
0 2 * * * ~/scripts/backup-bookmarks.sh
```

### 5. Bulk Operations

**Add multiple bookmarks from a file**:

Create `links.txt`:
```
https://example1.com
https://example2.com
https://example3.com
```

Import script:
```bash
while read url; do
  bookmark add "$url" --tag imported
done < links.txt
```

**Batch tag updates**:
```bash
# Rename tag across all bookmarks
bookmark tags rename old-tag new-tag --yes

# Add tag to all bookmarks matching search
for url in $(bookmark search "python" --json | jq -r '.bookmarks[].url'); do
  bookmark edit "$url" --add-tag python-resource
done
```

---

## Troubleshooting

### Bookmark file corrupted

**Symptom**: Error message about corrupted data file

**Solution**:
```bash
# Try to restore from backup
cd ~/.bookmarks
cp bookmarks.json.backup bookmarks.json

# If no backup exists, try to manually fix JSON
# Open in text editor and fix syntax errors
code ~/.bookmarks/bookmarks.json
```

### Can't find bookmark I just added

**Check**:
1. Verify it was actually added: `bookmark list --sort modified`
2. Check if typo in search: `bookmark list | grep -i <term>`
3. Try searching by tag instead: `bookmark list --tag <tag>`

### Duplicate detection not working

**Cause**: URLs might have slight differences (http vs https, trailing slash, query params)

**Solution**: Normalize URLs before adding or search for similar URLs:
```bash
bookmark search example.com
```

### Import from browser not working

**Check**:
1. Ensure file is HTML format exported from browser
2. Try: `bookmark import bookmarks.html --format html`
3. Check file contents: Should start with `<!DOCTYPE NETSCAPE-Bookmark-file-1>`

---

## Next Steps

- **Read full documentation**: See `README.md` for complete feature list
- **Explore advanced search**: Combine tags and text queries for powerful filtering
- **Set up automation**: Create shell scripts for common bookmark operations
- **Contribute**: Report issues or contribute at https://github.com/your-org/bookmark-organizer

---

## Quick Reference

| Command | Description | Example |
|---------|-------------|---------|
| `bookmark add <url>` | Add bookmark | `bookmark add https://example.com --tag python` |
| `bookmark list` | List all | `bookmark list --tag python` |
| `bookmark search <query>` | Search text | `bookmark search tutorial` |
| `bookmark edit <url>` | Edit bookmark | `bookmark edit https://example.com --add-tag new` |
| `bookmark delete <url>` | Delete bookmark | `bookmark delete https://example.com --yes` |
| `bookmark tags list` | List all tags | `bookmark tags list --sort count` |
| `bookmark import <file>` | Import bookmarks | `bookmark import bookmarks.html` |
| `bookmark export <file>` | Export bookmarks | `bookmark export backup.json --tag work` |
| `bookmark --help` | Show help | `bookmark add --help` |

---

**Storage Location**: `~/.bookmarks/bookmarks.json`  
**Backup Location**: `~/.bookmarks/bookmarks.json.backup`  
**Config** (future): `~/.bookmarks/config.json`
