# TestRunner

Test runner providing a CLI for running test suites.

# Installation
TestRunner requires [Node.js](https://nodejs.org/) v8.9.0+ to run.
Once installed, you can follow the instructions below to execute commands.

# Usage

### Starting TestRunner
Navigate to the **dist** folder in this repository via the command line and start **TestRunner**:
```sh
$ cd dist
$ TestRunner.exe
Test runner version 1.0.0...
```

### Exit TestRunner
exit
```sh
exit
```

### Starting a new test run
start -n [testSuiteName]
```sh
start -n testSuite4
```

### Get the status of a test run
status -i [testRunId]
```sh
status -i d822c41a-610b-44f8-9d68-37633cb0479c
```

### Get the results of a test run
results -i [testRunId]
```sh
results -i d822c41a-610b-44f8-9d68-37633cb0479c
```

### Cancel a test run
cancel -i [testRunId]
```sh
cancel -i d822c41a-610b-44f8-9d68-37633cb0479c
```