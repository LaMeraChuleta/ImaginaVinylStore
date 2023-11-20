import excel_to_db_service

path_excel = "catalogo_db.xlsx"
sql_string_conn = "DRIVER=SQL Server;SERVER=localhost;DATABASE=test;UID=sa;PWD=VacaLoca69"

excel_to_db = excel_to_db_service.ExcelToDbService(path_excel, sql_string_conn)

excel_data = excel_to_db.get_data_for_excel()
excel_to_db.insert_catalog_data_to_database(excel_data)