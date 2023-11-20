import pyodbc
import pandas
import datetime

class DataBaseService:
                
        connection = None
        connection_string = None        

        def __init__(self, connection_string):      
            self.connection_string = connection_string

        def connect_database(self):
            try:
                self.connection = pyodbc.connect(self.connection_string)
                return self.connection
            except Exception as e:
                print(f"Error al conectar a la base de datos: {str(e)}")
                return None                

        def close_connection(self):
            try:
                if self.connection:
                    self.connection.close()
            except Exception as e:
                print(f"Error al cerrar la base de datos: {str(e)}")

        def insert_catalog_music(self, new_catalog_music):
            try:
                query_insert_catalog_music = f'''
                INSERT INTO MusicCatalog (Title, ArtistId, GenreId, FormatId, 
                    PresentationId, Country, [Year], StatusCover, StatusGeneral,
                    Price, Matrix, Label, CreateAt, ActiveInStripe)
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?)
                '''       

                self.connection.cursor().execute(query_insert_catalog_music, 
                    new_catalog_music["Title"],
                    self.get_id_artist(new_catalog_music["Artist"]),
                    self.get_id_genre(new_catalog_music["Genre"]),
                    self.get_id_format(new_catalog_music["Format"]),
                    1,#self.get_id_presentation(new_catalog_music["Presentation"], self.get_id_format(new_catalog_music["Format"])),
                    new_catalog_music["Country"],
                    new_catalog_music["Year"],
                    10,#StatusCover
                    10,#StatusGeneral
                    new_catalog_music["Price"],
                    new_catalog_music["Matrix"],
                    new_catalog_music["Label"],
                    datetime.datetime.now(),
                    False)
                self.connection.commit()      

            except Exception as e:
                print(str(e))

        def get_id_artist(self, artist_text):
            try:
                artist_df = pandas.read_sql_query("SELECT * FROM Artist", pyodbc.connect(self.connection_string))
                if artist_df['Name'].isin([artist_text]).any():
                    return int(artist_df.loc[artist_df['Name'] == artist_text, 'Id'].values[0])    
                else:        
                    return int(self.create_artist(artist_text))
            except Exception as e:
                print(str(e))
    
        def create_artist(self, artist_name):                                               
            try:                                            
                self.connection.cursor().execute("INSERT INTO Artist (Name, Country) VALUES (?, ?)", artist_name, "S/A")
                return self.get_last_id_insert()
            except Exception as e:
                print(str(e))
        
        def get_id_genre(self, genre_text):
            try:
                genre_df = pandas.read_sql_query("SELECT * FROM Genre", self.connection)
                if genre_df['Name'].isin([genre_text]).any():
                    return int(genre_df.loc[genre_df['Name'] == genre_text, 'Id'].values[0])    
                else:        
                    return int(self.create_genre(genre_text))
            except Exception as e:
                print(str(e))
    
        def create_genre(self, genre_name):  
            try:                                           
                self.connection.cursor().execute("INSERT INTO Genre (Name) VALUES (?)", genre_name)
                self.connection.commit()            
                return self.get_last_id_insert()
            except Exception as e:
                print(str(e))
                
        def get_id_format(self, format_text):
            try:
                format_df = pandas.read_sql_query("SELECT * FROM Format", self.connection)
                if format_df['Name'].isin([format_text]).any():
                    return int(format_df.loc[format_df['Name'] == format_text, 'Id'].values[0])
                else:        
                    return int(self.create_format(format_text))
            except Exception as e:
                print(str(e))
    
        def create_format(self, format_name):                        
            try:                                                
                self.connection.cursor().execute("INSERT INTO Format (Name) VALUES (?)", format_name)
                self.connection.commit()            
                return self.get_last_id_insert() 
            except Exception as e:
                print(str(e))                

        def get_id_presentation(self, presentation_text, format_id):
            try:
                presentation_df = pandas.read_sql_query("SELECT * FROM Presentation", self.connection)
                if presentation_df['Name'].isin([presentation_text]).any():
                    return presentation_df.loc[presentation_df['Name'] == presentation_text, 'Id'].values[0]
                else:        
                    return self.create_presentation(presentation_text, format_id)
            except Exception as e:
                print(str(e))
    
        def create_presentation(self, presentation_name, format_id):            
            try:                
                self.connection.cursor().execute("INSERT INTO Presentation (Name, FormatId) VALUES (?, ?)", presentation_name, format_id)
                self.connection.commit()            
                return self.get_last_id_insert()
            except Exception as e:
                print(str(e))

        def get_last_id_insert(self):
            try:
                cursor = self.connection.cursor()
                cursor.execute("SELECT @@IDENTITY  AS Id")
                return cursor.fetchone().Id
            except Exception as e:
                print(str(e))