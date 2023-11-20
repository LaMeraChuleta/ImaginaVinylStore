import pandas
import data_base_service

class ExcelToDbService:
    
    excel_path = None
    data_base_service = None    

    def __init__(self, excel_path, connection_string):            
        self.excel_path = excel_path
        self.data_base_service = data_base_service.DataBaseService(connection_string)

    def get_data_for_excel(self):        
        return pandas.read_excel(self.excel_path)                    

    def insert_catalog_data_to_database(self, data_excel):        
        try:
            self.data_base_service.connect_database()
            
            for index, col in data_excel.iterrows():         
                self.data_base_service.insert_catalog_music(col)

        except Exception as e:
            print(f"Error al insertar datos en catalog music: {str(e)}")
