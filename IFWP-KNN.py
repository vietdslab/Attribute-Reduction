# -*- coding: utf-8 -*-
"""
Created on Mon Jan 18 09:25:52 2021

@author: Administrator
"""

from sklearn import metrics
import pandas as pd
import time
from sklearn.model_selection import KFold, cross_val_score
from sklearn import svm
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.tree import DecisionTreeClassifier
from sklearn import neighbors
from itertools import chain, combinations

def powerset(s):
    return chain.from_iterable(combinations(s, r) for r in range(1,len(s)+1))
"""

wine='D:\Giang Day\DO-IT\Tap mo\DataTN\wine.csv'

heart='D:\Giang Day\DO-IT\Tap mo\DataTN\heart.csv'

stalog='D:\Giang Day\DO-IT\Tap mo\DataTN\stalog.csv'

segment='D:\Giang Day\DO-IT\Tap mo\DataTN\segment.csv'

hepatits='D:\Giang Day\DO-IT\Tap mo\DataTN\hepatits.csv'

horse='D:\Giang Day\DO-IT\Tap mo\DataTN\horse.csv'

wdbc='D:\Giang Day\DO-IT\Tap mo\DataTN\wdbc.csv'

wpbc='D:\Giang Day\DO-IT\Tap mo\DataTN\wpbc.csv'

iono='D:\Giang Day\DO-IT\Tap mo\DataTN\iono.csv'

libra='D:\Giang Day\DO-IT\Tap mo\DataTN\libra.csv'

ecoli='D:\Giang Day\DO-IT\Tap mo\DataTN\ecoli.csv'

tra='D:\Giang Day\DO-IT\Tap mo\DataTN\A.csv'

trb='D:\Giang Day\DO-IT\Tap mo\DataTN\B.csv'

trc='D:\Giang Day\DO-IT\Tap mo\DataTN\C.csv'

"""

segment='D:\Giang Day\DO-IT\Tap mo\DataTN\segment.csv'

wine='D:\Giang Day\DO-IT\Tap mo\Data\Wine\wine.csv'

heart='D:\Giang Day\DO-IT\Tap mo\Data\heart\heart.csv'

australian='D:\Giang Day\DO-IT\Tap mo\Data\_australian\_australian.csv'

hepatits='D:\Giang Day\DO-IT\Tap mo\Data\_hepatits\_hepatitsP.csv'

horse='D:\Giang Day\DO-IT\Tap mo\Data\horse\horse.csv'

wdbc='D:\Giang Day\DO-IT\Tap mo\Data\wdbc\wdbc.csv'

wpbc='D:\Giang Day\DO-IT\Tap mo\Data\wpbc\wpbcP.csv'

iono='D:\Giang Day\DO-IT\Tap mo\Data\iono\iono.csv'

qa='D:\Giang Day\DO-IT\Tap mo\Data\qa\qa.csv'

libra='D:\Giang Day\DO-IT\Tap mo\Data\libra\libra.csv'

trc='D:\Giang Day\DO-IT\Tap mo\DataTN\C.csv'

pck='D:\Giang Day\DO-IT\Tap mo\DataTN\Packinson.csv'

sona='D:\Giang Day\DO-IT\Tap mo\DataTN\sonar.csv'

hfrc='D:\Giang Day\DO-IT\Tap mo\DataTN\HFCR2.csv'

hcv='D:\Giang Day\DO-IT\Tap mo\DataTN\HCV2.csv'

mv11cl='D:\Giang Day\DO-IT\Tap mo\DataTN\MV11CL.csv'

bhp='D:\Giang Day\DO-IT\Tap mo\DataTN\BHP1.csv'

R='a12,a1,a2,a13,a8,a10'.split(',')
rss=wine


knum = 10
k_fold = KFold(n_splits=knum, shuffle=True)
#clf1 = DecisionTreeClassifier()
clf2 = neighbors.KNeighborsClassifier(n_neighbors = 10, p = 2)
#clf2 = LinearDiscriminantAnalysis()
#clf2 = svm.SVC(kernel='linear')
#-----------------------------------
dataTN=pd.read_csv(rss, sep=',')
dataTN=dataTN.drop(columns=['U'])
Cname=dataTN.columns[0:dataTN.shape[1]-1]
X=dataTN[Cname].values
#---------------------------------

y=dataTN['d'].values
Lraw1=[]
Lraw2=[]
for i in range(0,10):
    sumR1=0
    sumR2=0
    for train_indices, test_indices in k_fold.split(X):
            #clf1.fit(X[train_indices],y[train_indices])
            clf2.fit(X[train_indices],y[train_indices])
            #y_pred1 = clf1.predict(X[test_indices])
            y_pred2 = clf2.predict(X[test_indices])
            #sumR1 += metrics.accuracy_score(y[test_indices], y_pred1)
            sumR2 += metrics.accuracy_score(y[test_indices], y_pred2)
    #Lraw1.append(sumR1/knum)
    Lraw2.append(sumR2/knum)
#dfLraw1=pd.DataFrame(Lraw1)
dfLraw2=pd.DataFrame(Lraw2)
#TB=round(float(dfLraw1.sum()/dfLraw1.count(),3)
#strraw='Accuracy-Cart: '+str(dfLraw1.sum()/dfLraw1.count()) +'\nAccuracy-KNN: '+str(dfLraw2.sum()/dfLraw2.count())
#print(strraw)
print('Accuracy on original dataset: '+str(list(Cname)))
#print('C45='+str(float(dfLraw1.sum()/dfLraw1.count())))
print('KNN='+str(float(dfLraw2.sum()/dfLraw2.count())))
#print('Accuracy TB= '+str(TB))
print('-----------------------------------')

Lraw1=[]
Lraw2=[]
XR=dataTN[R].values
for i in range(0,10):
    sumR1=0
    sumR2=0
    for train_indices, test_indices in k_fold.split(XR):
            #clf1.fit(XR[train_indices],y[train_indices])
            clf2.fit(XR[train_indices],y[train_indices])
            #y_pred1 = clf1.predict(XR[test_indices])
            y_pred2 = clf2.predict(XR[test_indices])
            #sumR1 += metrics.accuracy_score(y[test_indices], y_pred1)
            sumR2 += metrics.accuracy_score(y[test_indices], y_pred2)
    #Lraw1.append(sumR1/knum)
    Lraw2.append(sumR2/knum)
#dfLraw1=pd.DataFrame(Lraw1)
dfLraw2=pd.DataFrame(Lraw2)
TB=round(float(dfLraw2.sum()/dfLraw2.count()),3)
print('Accuracy on Filter reduct: '+str(list(R)))
#print('C45='+str(float(dfLraw2.sum()/dfLraw2.count())))
print('KNN='+str(float(dfLraw2.sum()/dfLraw2.count())))
#print('Accuracy TB= '+str(TB))
print('-----------------------------------')

t0=time.time()
for i in range(0,len(R)):
    XRFS=dataTN[R[0:i+1]].values
    LFS1=[]
    LFS2=[]
    for j in range(0,10):
        sumR1=0
        sumR2=0
        for train_indices, test_indices in k_fold.split(XRFS):
                #clf1.fit(XRFS[train_indices],y[train_indices])
                clf2.fit(XRFS[train_indices],y[train_indices])
                #y_pred1 = clf1.predict(XRFS[test_indices])
                y_pred2 = clf2.predict(XRFS[test_indices])
                #sumR1 += metrics.accuracy_score(y[test_indices], y_pred1)
                sumR2 += metrics.accuracy_score(y[test_indices], y_pred2)
        #LFS1.append(sumR1/knum)
        LFS2.append(sumR2/knum)
    #dfLraw1=pd.DataFrame(LFS1)
    dfLraw2=pd.DataFrame(LFS2)
    if float(dfLraw2.sum()/dfLraw2.count())>TB:
        #TB=round(float(dfLraw2.sum()/dfLraw2.count()),3)
        print('Accuracy on Wrapper reduct: '+str(R[0:i+1]))
        #print('C45='+str(float(dfLraw1.sum()/dfLraw1.count())))
        print('KNN='+str(float(dfLraw2.sum()/dfLraw2.count())))
        #print('Accuracy TB= '+str(TB))
    #strFS='Accuracy-Cart: '+str(dfLraw1.sum()/dfLraw1.count()) +'\nAccuracy-KNN: '+str(dfLraw2.sum()/dfLraw2.count())
t1=time.time()
tt=t1-t0
print(tt)