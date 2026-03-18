# 🎮 Catch it! VR: Integrazione tra Realtà Virtuale e Blockchain

**Catch it! VR** è un progetto di tesi sperimentale che unisce l'immersività della **Virtual Reality** con la sicurezza e la trasparenza della **Blockchain**. Il sistema permette di giocare in un ambiente VR e registrare punteggi e statistiche in modo immutabile su un registro distribuito.

## 🎯 Obiettivi del Progetto
* **Immersività:** Sviluppo di un'esperienza VR dinamica in ambiente Unity.
* **Data Integrity:** Utilizzo della Blockchain per prevenire il cheating e garantire la tracciabilità dei record.
* **Interoperabilità:** Creazione di un bridge tra l'engine di gioco (C#) e gli Smart Contract (Solidity).

## 🛠️ Tech Stack
* **Game Engine:** Unity (C#) con supporto per **Meta Quest 3** (Standalone e PCVR).
* **Blockchain:** Ethereum (Local Network tramite **Truffle** e **Ganache**).
* **Smart Contracts:** Linguaggio **Solidity** per la logica di salvataggio punteggi.
* **Librerie:** Nethereum / Web3.js per la comunicazione tra Unity e la rete Ethereum.

## 🏗️ Architettura del Sistema
Il progetto si basa su un'interazione costante tra il visore VR e la blockchain:
1. **Gameplay:** L'utente interagisce nel mondo VR; al termine della sessione, i dati vengono pacchettizzati.
2. **Transazione:** Unity invia una chiamata allo Smart Contract tramite un account Ethereum associato al profilo utente.
3. **Validazione:** Il punteggio, il timestamp e l'hash dei movimenti vengono registrati sulla blockchain locale.

## 📊 Test e Performance
La tesi include un'analisi dettagliata del carico hardware (CPU/GPU) confrontando le modalità **Standalone** e **PCVR**, oltre alla misurazione dei tempi di latenza delle transazioni sulla blockchain (mediamente tra **65-81 ms** dopo il setup iniziale).

## 📂 Contenuto della Repository
* `/Scripts/`: Logica di gioco in C# e script di integrazione Web3.
* `/Contracts/`: Smart Contract in Solidity per la gestione della classifica.
* `/Migrations/`: Script di deployment per Truffle.

---
*Tesi di Laurea Triennale in Informatica - Università degli Studi di Messina (A.A. 2024-2025).*
*Relatore: Prof. Antonino Galletta*
