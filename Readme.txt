################################################################################
1. This application needs some nuget packegs to run.
2. Restore all packages to run this application.


###############################################################################
1. This application is small that's why I have used built in IOC container. Other IOC container like Unity, Autofac, Structure also we can use.
2. I have used serilog for loggig all information. This is really good tool for logging into text file, database or api also.
3. I have used Xunit for writting test cases. I could use moq also if this application has database realated stuff.
4. I have used dependency injection pattern to build this application which is helpful for writting test cases. 
5. I could use automapper also at some of places but application is small that's why I have not used it.
6. Application is small that's why I have kept everthing in one project otherwise I could split into multiple project.



##################### What more I can do which I have not done becuase fo time constrain ###############################
1. Write more test cases for tesiting.
2. Some of code need to refactor litte more.
3. I could use some other nuget packages to structure the application 



##################################### How to run the application #####################################
1. Please use resource folder to get files for testing the application.
3. Make sure file has correct data to get correct output.