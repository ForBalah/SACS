General notes:
--------------
As of the time of writing this, data access is via EF6 model first.

The database file is a single access database file, designed to work in SQL express 2012.

Connecting to the database file:
--------------------------------
- Under Tools, click "Connect to Database"
- click "Change" and select "Microsoft SQL Database File"
- Browse for the file in the solution. Make sure it's first checked out.
- Use Windows Authentication.
- Click Connect and the it should now appear in the Server Explorer view.

See https://msdn.microsoft.com/en-us/library/ms239722.aspx for more information.

Be sure to disconnect after using it to prevent build issues.

Updating the database:
----------------------
After fixing up the edmx, perform the following:
- check out Entities\SACSEntities.edmx.sql
- check out SACS.mdf
- right-click on the surface and click Generate Database From Model. The path should point to Entities\SACSEntities.edmx.sql
- copy the script to a new query window pointing to the attached SACS.mdf file.
- remove the USE at the top so that it runs

Troubleshooting Builds:
-----------------------
Remember to disconnect from the database in the Object explorer when building as the database gets copied to the output directory and this can only happen if there are no processes locking the file.