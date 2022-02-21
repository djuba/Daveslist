### Brief

After a few awkward experiences trying to sell your car on Craigslist, including that time you received a strange offer from a complete stranger which was totally unrelated to your old Toyota Camry, you decide to that there has to be a better way. So, you build it: *Daveslist - for used cars.*

Now it's time to build the API. In the application, users should be able to interact with each other by creating listings, replying to listings, and sending private messages. Users should also have different permissions within the app depending on their status (e.g. anonymous site visitor, registered user, moderator, and admin.) 

### Tasks

- Implement assignment using:
    - Language: *C#*
    - Framework: *.Net*
- Every site visitor should be able to see public listings in public categories.
- Registered users should be able to see all categories (both public and private) and all listings.
- Registered users should be able to create a listing, with the following options:
    - Select a category from existing categories
    - Select whether the listing should be public or private (visible to registered users only)
- Each listing contains title, content, and pictures (5000 characters limit for content, up to 10 pictures per listing)
- Replies can be text only (255 chars max)
- Registered users should be able to edit or delete their own listings.
- Registered users should be able to reply to any listing unless the original post is older than 1 year (to prevent "necrobumping")
- Registered user can send a private message to other users
- Moderators should be able to temporarily hide (but not edit or delete) any listing.
- Moderators can create or delete categories (on deleting category all listings in that category should not be permanently deleted, but rather moved to "trash bin")
- Admin (superuser) can do everything the moderator can do, plus they can edit and delete any listing.

Your task is to build an HTTP API that will provide the functionality above. 
You should write unit tests for business logic. 
You are expected to design any other required models and routes for your API.

 ### Evaluation Criteria

 - *C#* best practices
 - Completeness: Did you include all features?
 - Correctness: Does the solution work in sensible, thought-out ways?
 - Maintainability: Is the code written in a clean, maintainable way?
 - Testing: Is the solution adequately tested?
 - Documentation: Is the API well-documented?

### CodeSubmit

Please organize, design, test and document your code as if it were
going into production - then push your changes to the master branch.
After you have pushed your code, you may submit the assignment on the
assignment page.

All the best and happy coding,

The SchoolPass Team