# WordDiff

A .Net class library for doing a word based text diff (instead of character based).

Uses [DiffPlex](https://github.com/mmanela/diffplex) internally to do a word-by-word diff, and merge both pieces of input together and marks the insertions and deletions with `<ins>` and `<del>` html tags.
