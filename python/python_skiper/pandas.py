import pandas as pd
import pyodbc
import datetime

class ExcelToDatabase:
    def __init__(self, excel_file, connection_string):
        self.excel_file = excel_file
        self.connection_string = connection_string
        self.connection = None

    def read_excel(self):
        try:
            self.df = pd.read_excel(self.excel_file)
            return True
        except Exception as e:
            print(f"Error al leer el archivo Excel: {str(e)}")
            return False

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
            self.connection.commit()
            print("Datos insertados en la base de datos con éxito.")
        except Exception as e:
            print(f"Error al insertar datos en la base de datos: {str(e)}")

    def close_connection(self):
        if self.connection:
            self.connection.close()

if __name__ == "__main__":
    excel_file = "catalogo_db.xlsx"
    connection_string = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"

    excel_to_db = ExcelToDatabase(excel_file, connection_string)

    if excel_to_db.read_excel() and excel_to_db.connect_database():
        excel_to_db.insert_data_to_database()
        excel_to_db.close_connection()
