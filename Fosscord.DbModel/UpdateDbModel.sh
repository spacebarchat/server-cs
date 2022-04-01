#!/bin/sh
echo '-- Db scaffold script - postgresql --'
echo '-- Made by The Arcane Brony --'
echo '-- Cleaning up previous scaffold... --'
rm -rf test/*
echo '-- Checking and asking for credentials... --'
if [ -z "$PG_HOST" ]
then
      read -rp 'Hostname: ' PG_HOST
      ASK_SAVE=1
fi
if [ -z "$PG_PORT" ]
then
      read -rp 'Port: ' PG_PORT
      ASK_SAVE=1
fi
if [ -z "$PG_USER" ]
then
      read -rp 'Username: ' PG_USER
      ASK_SAVE=1
fi
if [ -z "$PG_PASS" ]
then
      read -srp 'Password: ' PG_PASS
      ASK_SAVE=1
      echo
fi
if [ -z "$PG_DB_FOSSCORD" ]
then
      read -rp 'Database name: ' PG_DB_FOSSCORD
      ASK_SAVE=1
fi
if [ -n "$ASK_SAVE" ]
then
      read -rp 'Do you want to save these as user environment variables? (y/n) ' SAVE
      if [ "$SAVE" = "y" ]
      then  
           {
              echo "export PG_HOST=$PG_HOST";
              echo "export PG_PORT=$PG_PORT";
              echo "export PG_USER=$PG_USER";
              echo "export PG_PASS=$PG_PASS";
              echo "export PG_DB_FOSSCORD=$PG_DB_FOSSCORD"
           } >> ~/.profile
          echo 'Saved!'
          echo 'Type `. ~/.profile` to load them now.'
      fi
fi
echo '-- Importing db model from postgres... --'
dotnet ef dbcontext scaffold "server=$PG_HOST;username=$PG_USER;password=$PG_PASS;database=$PG_DB_FOSSCORD;port=$PG_PORT" "Npgsql.EntityFrameworkCore.PostgreSQL" -o Scaffold -f --data-annotations --namespace Fosscord.Db.Scaffold
echo '-- Listing untracked files --'
git ls-files Scaffold --exclude-standard --others
echo '-- Please add these tables to Db --'
echo '-- You should be able to copy paste from fosscordContext.cs --'
read -p 'Press enter when finished...'
read -p 'Enter a migration name: ' MIG_NAME
dotnet ef migrations add "$MIG_NAME" --context Db