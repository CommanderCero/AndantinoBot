import pandas as pd

### Read and combine data
heuristic_data = pd.read_csv("combined_heuristics.csv", sep=",", index_col="HashCode")
win_data = pd.read_csv("combined_winCounter.csv", sep=",", index_col="HashCode")

data = heuristic_data.join(win_data)
del heuristic_data
del win_data

### Collect features and calculate target
X = data.iloc[:,0:-3]
y = data.iloc[:,-3:]

### We do not need to consider draws
print("Are there any draw games: ", any(y["None"] > 0))
y = y.iloc[:,1:]
y["Black_Winrate"] = y["Black"] / (y["Black"] + y["White"])
y["White_Winrate"] = 1 - y["Black_Winrate"]

### get correlations of each features in dataset
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns

# build dataframe
df = X.join(y["White_Winrate"])
df = X.join(y["Black_Winrate"])

corrmat = df.corr()
top_corr_features = corrmat.index
plt.figure(figsize=(40,40))
#plot heat map
g=sns.heatmap(df[top_corr_features].corr(),cmap="RdYlGn")
plt.plot()
