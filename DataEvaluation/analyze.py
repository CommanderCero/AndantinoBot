import pandas as pd

# Read and combine data
heuristic_data = pd.read_csv("combined_heuristics.csv", sep=",", index_col="HashCode")
win_data = pd.read_csv("winCounter_20191013_203004.csv", sep=";", index_col="HashCode")

data = heuristic_data.join(win_data)
del heuristic_data
del win_data

