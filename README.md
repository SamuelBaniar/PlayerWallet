# PlayerWallet
<p align="center">
  Simple ASP.NET Core Rest Api for player's wallet management
</p>

## Usage
Note: Aplication has mocked data

**Examples**:  
Get Wallet Balance By Guid:
**GET** localhost:5000/api/wallet?guid=00000000-0000-0000-0000-000000000001

Create Wallet:  
**POST** localhost:5000/api/wallet?guid=00000000-0000-0000-0000-000000000003

Create Transaction:  
**POST** localhost:5000/api/transaction
body:  
```json
{
    "Guid": "00000000-0000-0000-0000-000000000001",
    "TransactionType": "stake",
    "Amount": 50,
    "WalletGuid": "00000000-0000-0000-0000-000000000002"
}
```

**POST** localhost:5000/api/transaction
body:  
```json
{
    "Guid": "00000000-0000-0000-0000-000000000002",
    "TransactionType": "win",
    "Amount": 50,
    "WalletGuid": "00000000-0000-0000-0000-000000000002"
}
```

**POST** localhost:5000/api/transaction
body:  
```json
{
    "Guid": "00000000-0000-0000-0000-000000000003",
    "TransactionType": "deposit",
    "Amount": 50,
    "WalletGuid": "00000000-0000-0000-0000-000000000002"
}
```
