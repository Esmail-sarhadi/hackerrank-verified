# React: Text Append

A HackerRank React question: two text fields, and a label that shows the
concatenation under `data-testid="final-text"`.

Files: `App.js` goes to `src/App.js`, `textField.js` goes to
`src/components/textField.js`.

Two details decide whether the tests pass:

- `onChange` must receive the raw event. The parent reads
  `event.target.value`; trimming inside the handler would also strip a space
  the user typed deliberately between the two fields.
- The three `data-testid` attributes (`first-text`, `second-text`,
  `final-text`) are what the grader queries. Without them the component renders
  fine and the tests still fail.

No automated test here: the grader runs this inside a React test harness, and
reproducing that harness would test my copy of it rather than the component.
The logic is two `useState` calls and a template literal, so there is nothing
for a brute-force check to disagree with.
