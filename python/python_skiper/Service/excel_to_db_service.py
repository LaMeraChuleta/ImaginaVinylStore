import pandas as pd
import pyodbc
import datetime
from excel_service import ExcelService

class ExcelToDbService:
    
    ExcelService = ExcelService
    connection = None

    def __init__(self, excel_path, connection_string):
        self.ExcelService = ExcelService(excel_path)
        self.connection_string = connection_string
        self.connection = None

    def read_excel(self):
        return self.ExcelService.Read()

    def connect_database(self):
        try:
            self.connection = pyodbc.connect(self.connection_string)
            return True
        except Exception as e:
            print(f"Error al conectar a la base de datos: {str(e)}")
            return False

    def insert_data_to_database(self):
        if self.connection is None:
            print("No hay conexión a la base de datos.")
            return

        try:
            cursor = self.connection.cursor()
            for index, row in self.df.iterrows():
                # Aquí realiza las inserciones en una sola transacción sin validaciones
                cursor.execute(
                    "INSERT INTO MusicCatalog (Title, ArtistId, GenreId, FormatId, PresentationId, Country, [Year], StatusCover, StatusGeneral, Price, Matrix, Label, CreateAt, ActiveInStripe) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    row["Title"], row["ArtistId"], row["GenreId"], row["FormatId"], row["PresentationId"],
                    row["Country"], row["Year"], row["StatusCover"], row["StatusGeneral"], row["Price"],
                    row["Matrix"], row["Label"], datetime.datetime.now(), False
                )            
            print("Datos insertados en la base de datos con éxito.")
        except Exception as e:
            print(f"Error al insertar datos en la base de datos: {str(e)}")
