import pandas as pd
import datetime
import pyodbc

connection_string : str = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
# Query a ejecutar
query_artist = "SELECT * FROM Artist"
query_genre = "SELECT * FROM Genre"
query_format = "SELECT * FROM Format"
query_presentation = "SELECT * FROM Presentation"
