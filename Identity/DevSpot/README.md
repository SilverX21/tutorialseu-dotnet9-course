## Add Identity pages

To add the Identity pages for the authentication, we need to take these steps:
- right click in the project > Add > New Scaffolded Item...
- We select Identity
- Then we select the pages we want and select our ApplicationDbContext

This will generate a new folder called "Areas" with Razor Pages

Then we need to add the following to the Program.cs to handle the razor pages: `app.MapRazorPages()`

## Razor Pages vs View Pages

View Pages: these work with the MVC architecture, they need a controller to handle the requests
Razor Pages: These have the code behind, that manages all of the requests