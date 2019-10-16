import pandas as pd
import glob

heuristic_files = glob.glob("./RawData/heuristics*.csv")
win_files = glob.glob("./RawData/winCounter*.csv")
heuristic_reader = lambda path: pd.read_csv(path, sep=";", index_col="HashCode")

# Merge the win counter data
combined_data = heuristic_reader(win_files[0])
for name in win_files[1:]:
    print("Combining ", name)
    data = heuristic_reader(name)
    combined_data = pd.concat([combined_data, data])
    combined_data = combined_data.groupby(combined_data.index).sum()
    
    del data
    
combined_data.to_csv("combined_winCounter.csv")