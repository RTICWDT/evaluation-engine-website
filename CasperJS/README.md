# Tests in CasperJS
===================

* For each waitForX() call, there is an assert. This means there are many more tests shown in TeamCity than are actual features being tested.
* Along with the above, there is an assert in each fail() handler. These will need to be removed as development continues.

## Actual Tests in CasperJS:

* Test LogOn
* Test ChangePassword functionality
* Test Edit features (edits the Organization field)
* Tests failed Edit (empty Organization field)
* Tests locking and unlocking of account
