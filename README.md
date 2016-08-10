# refaction
A terribly written Web API project that can be used as a test for potential C# applicants.  It's terrible on purpose, so that you can show us how we can improve it.

## Getting the refacted solution running

The refacted solution is based on SQL Server Express, no more data file. The Web Api application will try to initial a data base on the local instance of the SQL Server Express and it will load some sample data into the db at the fist time of the initialization. For all PUT, POST and DELETE routes, Web Api requires authentication first. To be authenticated as tester user, you need to add `AuthTick` into request header and set the value to `1F7A570C-9764-41E5-9F0E-212FA2C703AC` before sending it out. There are bunch of other enhancements, enjoy.

## Getting started for applicants

Fork this repository and make your changes to this project to make it better.  Simple.  There are no rules, except that we know that this project is very badly written, on purpose.  So, your job, should you choose to accept it, is to make the project better in any way you see fit.

To set up the project:

* Visual Studio 2015 is preferred.
* Open in VS.
* Restore nuget packages and rebuild.
* Run the project.

There should be these endpoints:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**
```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**
```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```

## Once you're done

Create a pull request back into this repository and describe, in as much detail as you feel necessary, what you have done to improve this project. Include your full name in the title as it appears on your CV so we can match it back to your job application. We'll take it from there and review.

Good luck!
