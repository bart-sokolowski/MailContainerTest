### Mail Container Test 

The code for this exercise has been developed to manage the transfer of mail items from one container to another for processing.

#### Process for transferring mail

- Lookup the container the mail is being transferred from.
- Check the containers are in a valid state for the transfer to take place.
- Reduce the container capacity on the source container and increase the destination container capacity by the same amount.

#### Restrictions

- A container can only hold one type of mail.


#### Assumptions

- For the sake of simplicity, we can assume the containers have an unlimited capacity.

### The exercise brief

The exercise is to take the code in the solution and refactor it into a more suitable approach with the following things in mind:

- Testability
- Readability
- SOLID principles
- Architectural design of the code

You should not change the method signature of the MakeMailTransfer method.

You should add suitable tests into the MailContainerTest.Test project.

#### Note: you must not use Github Copilot, ChatGPT, Google Bard etc to generate code for this exercise.
#### Code committed should be working, with passing tests, and able to be compiled and run on another machine; make sure you have referenced all packages / included any instructions necessary for someone else to run your tests.

There are no additional constraints, use the packages and approach you feel appropriate. Aim to spend no more than 2 hours on this exercise, but when planning your time bear in mind that your code needs to run and your tests should pass.

When completed, either submit a pull request to this repository (if you have a Github account), or zip up the completed code and
send it back to the recruiter who assigned you the test. 

Please either comment the PR or update the readme with any specific comments on any areas that are unfinished, and what you would cover given more time.

