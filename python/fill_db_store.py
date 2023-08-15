import pandas as pd
import pyodbc
import datetime

def get_id_artist(artist_df, artist_text):
    if artist_df['Name'].isin([artist_text]).any():
        return int(artist_df.loc[artist_df['Name'] == artist_text, 'Id'].values[0])
    else:        
        return int(create_artist(artist_text))
    

def create_artist(artist_name):
    connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
    connSql = pyodbc.connect(connection_string)
    cursor = connSql.cursor()
    # Datos del nuevo artista
    new_artist_country = "Mexico"
    # Consulta para insertar el nuevo artista
    insert_query = "INSERT INTO Artists (Name, Country) VALUES (?, ?)"
    cursor.execute(insert_query, artist_name, new_artist_country)
    connSql.commit()
    # Obtener el último ID insertado
    cursor.execute("SELECT SCOPE_IDENTITY() AS NewArtistID")
    new_artist_id = cursor.fetchone().NewArtistID
    connSql.close()
    return new_artist_id



def get_id_genre(genre_df, genre_text):
    if genre_df['Name'].isin([genre_text]).any():
        return int(genre_df.loc[genre_df['Name'] == genre_text, 'Id'].values[0])
    else:        
        return int(create_genre(genre_text))
    

def create_genre(genre_name):
    connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
    connSql = pyodbc.connect(connection_string)
    cursor = connSql.cursor()        
    # Consulta para insertar el nuevo artista
    insert_query = "INSERT INTO Genres (Name) VALUES (?)"
    cursor.execute(insert_query, genre_name)
    connSql.commit()
    # Obtener el último ID insertado
    cursor.execute("SELECT SCOPE_IDENTITY() AS NewGenreID")
    new_genre_id = cursor.fetchone().NewGenreID
    connSql.close()
    return new_genre_id


def get_id_format(format_df, format_text):
    if format_df['Name'].isin([format_text]).any():
        return int(format_df.loc[format_df['Name'] == format_text, 'Id'].values[0])
    else:        
        return int(create_format(format_text))
    

def create_format(format_name):
    connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
    connSql = pyodbc.connect(connection_string)
    cursor = connSql.cursor()        
    # Consulta para insertar el nuevo artista
    insert_query = "INSERT INTO Formats (Name) VALUES (?)"
    cursor.execute(insert_query, format_name)
    connSql.commit()
    # Obtener el último ID insertado
    cursor.execute("SELECT SCOPE_IDENTITY() AS NewFormatID")
    new_format_id = cursor.fetchone().NewFormatID
    connSql.close()
    return new_format_id



def get_id_presentation(presentation_df, presentation_text):
    if presentation_df['Name'].isin([genre_text]).any():
        return presentation_df.loc[presentation_df['Name'] == presentation_text, 'Id'].values[0]
    else:        
        return create_artist(presentation_text)
    

def create_presentation(presentation_name):
    connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
    connSql = pyodbc.connect(connection_string)
    cursor = connSql.cursor()        
    # Consulta para insertar el nuevo artista
    insert_query = "INSERT INTO Format (Name, FormatId) VALUES (?, ?)"
    cursor.execute(insert_query, presentation_name, 1)
    connSql.commit()
    # Obtener el último ID insertado
    cursor.execute("SELECT SCOPE_IDENTITY() AS NewFormatID")
    new_artist_id = cursor.fetchone().NewFormatID
    connSql.close()
    return new_artist_id


connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
# Query a ejecutar
query_artist = "SELECT * FROM Artists"
query_genre = "SELECT * FROM Genres"
query_format = "SELECT * FROM Formats"
query_presentation = "SELECT * FROM Presentations"

try:
    # Intentar establecer la conexión y ejecutar la consulta de los catalogos
    connSql = pyodbc.connect(connection_string)    
    artist_df = pd.read_sql_query(query_artist, connSql)
    genre_df = pd.read_sql_query(query_genre, connSql)
    formats_df = pd.read_sql_query(query_format,connSql)
    presentations_df = pd.read_sql_query(query_presentation,connSql)
    connSql.close()
    
    #leemo sel excel
    data_excel = pd.read_excel("catalogo_db.xlsx")    
    print(data_excel)
    
    for index, col in data_excel.iterrows():

        Title = col["Title"]
        ArtistName =  get_id_artist(artist_df, col["Artist"])
        GenreId = get_id_genre(genre_df, col["Genre"])
        FormatId = get_id_format(formats_df, col["Format"])
        PresentationId = 1
        country = col["Country"]
        Year_Db = col["Year"]
        StatusCover = 10
        StatusGeneral = 10
        Price = col["Price"]
        Matrix = col["Matrix"]
        Label = col["Label"]
        CreateAt = datetime.datetime.now()    

        
        test = f'''
        INSERT INTO MusicCatalogs (
            Title, 
            ArtistId, 
            GenreId, 
            FormatId, 
            PresentationId,
            Country,
            [Year],
            StatusCover,
            StatusGeneral,
            Price,
            Matrix,
            Label,
            CreateAt)
        VALUES(
            '{Title}',
            {ArtistName},
            {GenreId},
            {FormatId},
            {PresentationId},
            '{country}',
            {Year_Db},
            {StatusCover},
            {StatusGeneral},
            {Price},
            '{Matrix}',
            '{Label}',
            '{CreateAt}')
        '''

        query_insert_catalog_music = f'''
        INSERT INTO MusicCatalogs (
            Title, 
            ArtistId, 
            GenreId, 
            FormatId, 
            PresentationId,
            Country,
            [Year],
            StatusCover,
            StatusGeneral,
            Price,
            Matrix,
            Label,
            CreateAt)
        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)
        '''

        connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"
        connSql = pyodbc.connect(connection_string)
        cursor = connSql.cursor()        
        # Consulta para insertar el nuevo artista    
        print(test)    

        cursor.execute(query_insert_catalog_music, 
            Title, 
            ArtistName,
            GenreId , 
            FormatId,
            PresentationId,
            country,
            Year_Db, 
            StatusCover,
            StatusGeneral,
            Price,
            Matrix,
            Label,
            CreateAt)
        connSql.commit()

        print(query_insert_catalog_music)
    
    

except pyodbc.Error as e:
    print(f"Ocurrió un error: {e}")

