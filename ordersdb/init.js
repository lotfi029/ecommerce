db = db.getSiblingDB('eCommerceOrder');
db.createCollection('Orders');

print('Creating indexes for Orders collection...');

db.Orders.createIndex({ "userId": 1 }, { name: "idx_userId" });

db.Orders.createIndex({ "createdAt": -1 }, { name: "idx_createdAt_desc" });

db.Orders.createIndex({ "userId": 1, "createdAt": -1 }, { name: "idx_userId_createdAt" });

db.Orders.createIndex({ "totalBill": 1 }, { name: "idx_totalBill" });

db.Orders.createIndex({ "orderItems.ProductId": 1 }, { name: "idx_orderItems_productId" });

print('Indexes created successfully!');

print('Inserting sample orders...');

const sampleOrders = [
    {
        "_id": "01933b5e-8f2a-7c3d-9e4f-123456789001",
        "userId": "019736b6-4564-73a0-a6d6-39e948775fd8",
        "createdAt": new Date("2024-01-15T10:30:00Z"),
        "totalBill": 156.97,
        "orderItems": [
            {
                "_id": "098e6b64-2909-4419-9cea-c4af54730d1b",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 29.99,
                "quantity": 2,
                "totalPrice": 59.98
            },
            {
                "_id": "64a7a4d6-384f-480a-99fb-5509f5ee26c0",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 96.99,
                "quantity": 1,
                "totalPrice": 96.99
            }
        ]
    },
    {
        "_id": "01933b5e-8f2a-7c3d-9e4f-123456789002",
        "userId": "019736b6-4564-73a0-a6d6-39e948775fd8",
        "createdAt": new Date("2024-01-16T14:45:00Z"),
        "totalBill": 75.50,
        "orderItems": [
            {
                "_id": "bb73fa47-8c89-4763-94e4-6d83e6f91e60",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 25.00,
                "quantity": 3,
                "totalPrice": 75.00
            },
            {
                "_id": "bfa9f091-12f2-439c-89a6-6e84664cefb6",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 0.50,
                "quantity": 1,
                "totalPrice": 0.50
            }
        ]
    },
    {
        "_id": "01933b5e-8f2a-7c3d-9e4f-123456789003",
        "userId": "019736b6-4564-73a0-a6d6-39e948775fd8",
        "createdAt": new Date("2024-01-17T09:15:00Z"),
        "totalBill": 199.99,
        "orderItems": [
            {
                "_id": "b3d92683-2c72-439b-8616-ae65574bd3f7",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 199.99,
                "quantity": 1,
                "totalPrice": 199.99
            }
        ]
    },
    {
        "_id": "01933b5e-8f2a-7c3d-9e4f-123456789004",
        "userId": "019736b6-4564-73a0-a6d6-39e948775fd8",
        "createdAt": new Date("2024-01-18T16:20:00Z"),
        "totalBill": 45.48,
        "orderItems": [
            {
                "_id": "e3181100-79ae-4688-81e1-d6e43a10ea21",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 29.99,
                "quantity": 1,
                "totalPrice": 29.99
            },
            {
                "_id": "bfe3e91c-416b-4540-afe9-b258c3efc463",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 15.49,
                "quantity": 1,
                "totalPrice": 15.49
            }
        ]
    },
    {
        "_id": "01933b5e-8f2a-7c3d-9e4f-123456789005",
        "userId": "019736b6-4564-73a0-a6d6-39e948775fd8",
        "createdAt": new Date("2024-01-19T11:00:00Z"),
        "totalBill": 320.00,
        "orderItems": [
            {
                "_id": "d2fd5f32-c561-469f-b719-40a2c802cc6d",
                "ProductId": "01976f27-85d2-7b97-9983-9d4a8a8ad661",
                "unitPrice": 80.00,
                "quantity": 4,
                "totalPrice": 320.00
            }
        ]
    }
];

db.Orders.insertMany(sampleOrders);
