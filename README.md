# BrixProject_Loans
The project contains 2 services:
**LoanService**
When updating / creating a new loan, the service sends a message to the rules service that will check whether the loan complies with the rules of the requested loan provider. After the rules service has checked, it gets an answer from the rules service and updates the loan status. In addition, there is an option for the administrator to confirm a particular rule that failed.
**Rules Service**
This service checks if the loan complies with the rules of its loan provider (the relationship between the rules is an AND relationship (according to the requirements), so it is not represented in a tree. If it was an OR relationship I would have realized it in a tree)
The service receives an Excel file from the provider (the file has a const structure: parameter, condition and value, a sample file is attached here), and adds / updates its rules (the parameters of the rules are const), so in checking the loan it checks if it meets the rules.

How to get start?
-------
* Download BrixProject_Loans project
* Open the Project folder in VS
* Run it, and use by the Swagger UI page
  The Swagger Links are:  
   Loans Service 
    -  [https://localhost:44389/index.html](https://localhost:44389/index.html)
    -  [https://localhost:44389/swagger/LoansApiSpecification/swagger.json](https://localhost:44389/swagger/LoansApiSpecification/swagger.json)
   Rules Service 
    -  [https://localhost:44359/index.html](https://localhost:44359/index.html)
    -  [https://localhost:44359/swagger/RulesApiSpecification/swagger.json](https://localhost:44359/swagger/RulesApiSpecification/swagger.json)
* In order to add or update loan rules, upload (via the swagger) an excel file.
  Attached here an example file - rules.xlsx .
