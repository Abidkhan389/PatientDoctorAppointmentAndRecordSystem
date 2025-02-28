//Add-Migration command when we r working with n layer 
// please select startup project is PatientDoctor.API and in pkg manager console default proejct is PatientDoctor.Migrations
Add-Migration thirdMigration -Project PatientDoctor.Migrations -StartupProject PatientDoctor.API
//Update database command when we r working with n layer 
Update-Database -Project PatientDoctor.Migrations -StartupProject PatientDoctor.API




Add-Migration thirdMigration -Project PatientDoctor.Migrations -StartupProject PatientDoctor.API
Update-Database -Project PatientDoctor.Migrations -StartupProject PatientDoctor.API