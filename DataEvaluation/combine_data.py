import pandas as pd
import glob

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

# Find all states from which we have atleast 20 trajectories collected
valid_entries = combined_data[combined_data.sum(axis=1) >= 20].index

# Collect the corresponding heuristics from all our heuristic files, since they are split between them
heuristic_files = glob.glob("./RawData/heuristics*.csv")

heuristic_data = heuristic_reader(heuristic_files[0])
heuristic_data = heuristic_data[heuristic_data.index.isin(valid_entries)]
for name in heuristic_files[1:]:
    print("Collecting heuristics from ", name)
    data = heuristic_reader(name)
    data = data[data.index.isin(valid_entries)]
    
    heuristic_data = pd.concat([data, heuristic_data])
    duplicate_mask = heuristic_data.index.duplicated(keep="first")
    heuristic_data = heuristic_data.loc[~duplicate_mask]
    
    del data
    del duplicate_mask
    
    if len(valid_entries) == len(heuristic_data):
        break
    
heuristic_data.to_csv("combined_heuristics.csv")
