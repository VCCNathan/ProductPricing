## **Scenario: Product Pricing API**

The developer will create a **Product Pricing API** that allows clients to:

- Retrieve a list of products with their prices.
- Apply discounts to products.
- Update prices.
- Retrieve a price history of a product.

### **Requirements**

#### 1. **API Endpoints**

**GET /api/products**

- **Description**: Retrieve a list of products with their current prices.
- **Response**:
  ```json
  [
    {
      "id": 1,
      "name": "Product A",
      "price": 100.0,
      "lastUpdated": "2024-09-26T12:34:56"
    },
    {
      "id": 2,
      "name": "Product B",
      "price": 200.0,
      "lastUpdated": "2024-09-25T10:12:34"
    }
  ]
  ```

**GET /api/products/{id}**

- **Description**: Retrieve a product's price history.
- **Response**:
  ```json
  {
    "id": 1,
    "name": "Product A",
    "priceHistory": [
      { "price": 120.0, "date": "2024-09-01" },
      { "price": 110.0, "date": "2024-08-15" },
      { "price": 100.0, "date": "2024-08-10" }
    ]
  }
  ```

**POST /api/products/{id}/apply-discount**

- **Description**: Apply a discount to a product. The discount should be applied in percentage terms.
- **Request**:
  ```json
  {
    "discountPercentage": 10
  }
  ```
- **Response**:
  ```json
  {
    "id": 1,
    "name": "Product A",
    "originalPrice": 100.0,
    "discountedPrice": 90.0
  }
  ```

**PUT /api/products/{id}/update-price**

- **Description**: Update the price of a product.
- **Request**:
  ```json
  {
    "newPrice": 150.0
  }
  ```
- **Response**:
  ```json
  {
    "id": 1,
    "name": "Product A",
    "newPrice": 150.0,
    "lastUpdated": "2024-09-26T12:34:56"
  }
  ```

#### 2. **Technical Requirements**

- **.NET 8 Web API**
- **In-memory data storage**: For simplicity, use an in-memory collection to store the product and price information.
- **Pricing Model**: Product should have `Id`, `Name`, `Current Price`, `Price History`, and `Last Updated` fields.
- **Discounting Logic**: The discount is applied to the current price.
- **Unit Tests**: Write unit tests for the business logic (e.g., applying discounts, updating prices) using a unit testing framework like **nUnit.**

### **Submission Instructions**

- Provide a **GitHub repository** link with the source code.
- Ensure that the project builds successfully and all tests pass.
