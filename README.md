The Evaluation Engine project is made up of four repositories:

**Evaluation Engine Web UI:**
https://github.com/RTICWDT/evaluation-engine-website

**Evaluation Engine Service Broker:**
https://github.com/RTICWDT/evaluation-engine-service-broker

**Evaluation Engine Console Application:**
https://github.com/RTICWDT/evaluation-engine-console

**Evaluation Engine Statistical Component:**
<repo url here>

The Evaluation Engine Web UI project is the front end website for running reports. It communicates with it's own SQL Server database for controlling user accounts, hashed password histories and display options. This is included in this repo via SQL Source Control, in the 'Database' folder. There is also another database included in this project, in the 'WebRServerMessages' folder, where student IDs are uploaded to run reports and where results are written to by the statistical component to later be retrieved by the website. The data warehouse, where the student data (with student IDs replaced by study IDs) exist is included in the folder DataWarehouse. Finally, there is a Postgres database called 'Crosswalk' which maps hashed student IDs to study IDs.

The Service Broker project is used to communicate between the website database and the console application (mostly for hashing student IDs), as well as to send alert emails about reports finishing or erroring out. 

The Console Application is used for two purposes. First, it hashes student IDs when new data is recieved, to create a study ID that can be freely shared without revealing personally identifiable information. Second, it is used to hash student IDs when they are submitted via the website, so those study IDs can be transmitted and used in calcuations by the Statistical Component. 

Finally, the Statistical Component is where the actual calculations occur. It is deployed to its own R server, with tasks managed by a Gearman instance.

### Setup:
* Check out and build this application, and update the web.config with real values for the following:
	* SMTP email info in mailSettings
	* RServerURL -> URL of R server project
	* EvalEngineConnectionString -> website database
	* EEMessengerConnectionString -> WebRServerMessages database
	
* Deploy each of the four databases necessary for running the Evaluation Engine website. 
	* Database -> website database, committed using SQL Source Control
	* WebRServerMessagesDatabase -> communication between various components, uses SQL Source Control
	* crosswalk.sql -> postgres databse with one table, the crosswalk table
	* data-warehouse/data-warehouse.sql -> SQL Server data warehouse, will need to be filled with real data.

* For preparing data, examine the documents in the data-warehouse folder.