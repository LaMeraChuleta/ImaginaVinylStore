import os
import pandas

class ExcelService:

    dataExcel = pandas.DataFrame    
    path = ""

    def __init__(self, excel_path):        
        self.path = excel_path

    def Read(self):        
        if os.path.exists(self):
            try:                
                self.dataExcel = pandas.read_excel(self.path)                
                return True
            except Exception as e:
                print(f"Error al leer el archivo Excel: {str(e)}")            
                return False
        return False
    
    def IsFormatValid():
        return True

    def GetData(self):
        if self.dataExcel.any():
            return self.dataExcel   
        else:     
            print("No hay datos en el excel")
            return