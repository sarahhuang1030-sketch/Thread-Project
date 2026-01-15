I have published the project to Lab 1\Workshop04\Workshop04\bin\Release\net9.0\publish
I have the back up database in  Lab 1\Workshop04\Workshop04\db\backup

How to run
Open your terminal
Go to Lab 1\Workshop04\Workshop04


Type in the command below
docker compose up --build

Once it's down go to
http://localhost:8080/

Now in your Docker Desktop
You should see both thing in there
Container: workshop04
Image:workshop04-web
Volume: workshop04_sql_data

To double check the volume (the data is persists across restarts)

In the terminal typed
docker compose down

Then you should see 
[+] Running 4/4
 ✔ Container workshop04-web        Removed                                                                         0.0s
 ✔ Container workshop04-restore-1  Removed                                                                         0.1s
 ✔ Container travelexperts-db      Removed                                                                         0.1s
 ✔ Network workshop04_default      Removed  

and in Docker Desktop it shoulf turn into a grey circle

Then again in the terminal typed
docker compose up
(this time, the run time should be faster)

after everything is done, go to Docker Desktop
then you should see both has a green dot
Container: workshop04
Volume: workshop04_sql_data

**Please note: if you see the volume mark as "not in use", don't worry, the website should still run as expected**