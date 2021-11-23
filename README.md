# GS1API
GS1 Keyword Dictionary 

#  Steps to run the api : 
1. restore the packages 
2. update the database 'DefaultConnection' connection string from launchsettings.json file with you local db connection string
3. open the console on the WebApi project folder and update the database using 'dotnet ef database update'
4. run the application 

I have seeded some testing data so that you can test the app :
AdminUser creds : superadmin@gmail.com / 123Pa$$word!
BasicUser creds : basicuser@gmail.com / 123Pa$$word!

You can use those credentials to get the JWT token that you can use to consume the other sucured api endpoints.







