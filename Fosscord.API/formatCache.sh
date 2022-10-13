#!/bin/sh
rm -rfv ./cache_formatted/*
cp -rv ./cache/*.js ./cache_formatted/
cp -rv ./cache/*.css ./cache_formatted/
npx prettier --write ./cache_formatted/