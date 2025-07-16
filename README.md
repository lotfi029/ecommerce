# E-commerce Application

A microservices-based e-commerce application built with .NET, featuring product management, catalog browsing, order processing, authentication, and inventory tracking.

## Table of Contents

* Project Overview
* Architecture
* Services
* Technologies Used
* Prerequisites
* Setup Instructions
* Running the Application
* Contributing
* License

## Project Overview

This e-commerce application provides a scalable platform for online shopping using a microservices architecture. Each service operates independently, communicates asynchronously, and scales as needed. The application supports product management, catalog browsing, order processing, authentication, and inventory tracking.

## Architecture

The application uses a microservices architecture with the following components:

* **Product Service**: Manages product information (e.g., creation, updates, deletion).
* **Catalog Service**: Handles product browsing and searching.
* **Order Service**: Processes customer orders and maintains order history.
* **Auth Service**: Manages user authentication and authorization.
* **Inventory Service**: Tracks and updates product stock levels.
* **Message Broker**: RabbitMQ enables asynchronous communication.
* **Caching**: Redis improves performance by caching frequent data.

### Databases:

* PostgreSQL for relational data (orders, inventory).
* MongoDB for NoSQL data (products, catalog).

Each service is a separate .NET application, communicating via REST APIs and RabbitMQ events.

## Services

### Product Service

* **Responsibility**: Manages product data (name, description, price).
* **Database**: MongoDB
* **Endpoints**:

  * GET /api/products: Lists all products.
  * POST /api/products: Creates a new product.
  * PUT /api/products/{id}: Updates a product.
  * DELETE /api/products/{id}: Deletes a product.

### Catalog Service

* **Responsibility**: Provides product browsing and searching.
* **Database**: MongoDB
* **Endpoints**:

  * GET /api/catalog: Retrieves the product catalog.
  * GET /api/catalog/search?query={searchTerm}: Searches products by keyword.

### Order Service

* **Responsibility**: Handles order creation and history.
* **Database**: PostgreSQL
* **Endpoints**:

  * POST /api/orders: Creates a new order.
  * GET /api/orders/{userId}: Retrieves order history for a user.
* **Events**: Publishes `OrderCreated` event to RabbitMQ.

### Auth Service

* **Responsibility**: Manages authentication and authorization.
* **Technology**: Microsoft Identity (JWT-based).
* **Endpoints**:

  * POST /api/auth/register: Registers a new user.
  * POST /api/auth/login: Authenticates a user and returns a JWT token.

### Inventory Service

* **Responsibility**: Tracks and updates product stock.
* **Database**: PostgreSQL
* **Endpoints**:

  * GET /api/inventory/{productId}: Checks stock for a product.
  * PUT /api/inventory/{productId}: Updates stock levels.
* **Events**: Subscribes to `OrderCreated` event to update inventory.

## Technologies Used

* **Framework**: .NET 8 (ASP.NET Core)
* **Databases**:

  * PostgreSQL (v14+)
  * MongoDB (v5+)
* **Message Broker**: RabbitMQ (v3.9+)
* **Caching**: Redis (v6+)
* **Authentication**: Microsoft Identity (JWT)
* **Containerization**: Docker (optional)
* **CI/CD**: Configurable with GitHub Actions (not included)

## Prerequisites

* .NET 9 SDK
* PostgreSQL (v14+)
* MongoDB (v5+)
* RabbitMQ (v3.9+)
* Redis (v6+)
* Docker (optional)
* Code editor: Visual Studio Code or Visual Studio

## Setup Instructions

### Clone the Repository:

```bash
git clone https://github.com/lotfi029/ecommerce.git
cd ecommerce-app
```

### Configure Environment Variables:

Create a `.env` file in the root directory:

### Install Dependencies:

For each service (e.g., ProductService, OrderService):

```bash
cd ServiceName
dotnet restore
```

### Set Up Databases:

* **PostgreSQL**: Create an `ecommerce` database and run SQL scripts from `scripts/postgres`.
* **MongoDB**: Ensure MongoDB is running (no manual schema setup needed).

### Start RabbitMQ and Redis:

```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
docker run -d --name redis -p 6379:6379 redis
```

### Build and Run Services:

For each service:

```bash
cd ServiceName
dotnet build
dotnet run
```

## Running the Application

* Start all services using `dotnet run` or Docker.
* Use the Auth Service to register/login and obtain a JWT token.
* Interact with services using the token (e.g., via Postman or cURL).

### Example API call:

```bash
curl -H "Authorization: Bearer <your_jwt_token>" http://localhost:5001/api/products
```

## Contributing

We welcome contributions! Follow these steps:

1. Fork the repository.
2. Create a feature branch:

```bash
git checkout -b feature/your-feature
```

3. Commit changes:

```bash
git commit -m "Add your feature"
```

4. Push to the branch:

```bash
git push origin feature/your-feature
```

5. Open a Pull Request.

## License

This project is licensed under the MIT License.
