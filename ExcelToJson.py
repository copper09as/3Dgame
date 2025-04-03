import pandas as pd
import json

def excel_to_json(excel_file):
    # 读取Excel文件
    xls = pd.ExcelFile(excel_file)
    
    # 创建一个空字典存储每个Sheet的数据
    excel_data = {}
    
    # 遍历每个Sheet
    for sheet_name in xls.sheet_names:
        # 读取Sheet内容为DataFrame
        df = pd.read_excel(excel_file, sheet_name=sheet_name)
        
        # 将DataFrame转换为字典格式
        data_dict = df.to_dict(orient='records')
        
        # 将数据存入字典，以Sheet名为键
        excel_data[sheet_name] = data_dict
    
    # 将字典转换为JSON字符串
    json_data = json.dumps(excel_data, indent=4, ensure_ascii=False)
    
    return json_data

# 使用示例
excel_file_path = r'C:\Users\49469\Documents\test\123.xlsx'
json_output = excel_to_json(excel_file_path)
print(json_output)

with open(r'C:\Users\49469\Documents\test\output.json', 'w', encoding='utf-8') as f:
    f.write(json_output)
