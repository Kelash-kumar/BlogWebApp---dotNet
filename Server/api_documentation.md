# AuthDemo API Documentation

This document provides a detailed overview of the API endpoints available in the AuthDemo application, including their request and response formats.

- **Base URL**: `http://localhost:<port>/api`
- **Response Format**: All successful responses follow a standard envelope:

```json
{
  "success": boolean,
  "message": string,
  "data": T or null,
  "statusCode": integer
}
```

- **Paged results**: Methods returning lists wrap the data in a `PagedResult<T>`:
```json
{
  "data": [ /* items */ ],
  "totalRecords": integer,
  "pageNumber": integer,
  "pageSize": integer,
  "totalPages": integer,
  "hasNextPage": boolean,
  "hasPreviousPage": boolean
}
```

---

## 1. Authentication (`/api/Auth`)

### Login
- **Endpoint**: `POST /login`
- **Request Body (`LoginDto`)**:
```json
{
  "email": "string",
  "password": "string"
}
```
- **Response Data (`AuthResponseDto`)**:
```json
{
  "id": integer,
  "uid": "guid",
  "name": "string",
  "email": "string",
  "roles": ["string"],
  "token": "jwt-token",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### Register
- **Endpoint**: `POST /register`
- **Request Body (`RegisterDto`)**:
```json
{
  "name": "string",
  "email": "string",
  "password": "string",
  "confirmPassword": "string",
  "rolesIds": [integer]
}
```
- **Response Data**: Returns `AuthResponseDto`.

---

## 2. Users (`/api/User`)
*Authentication Required*

### Get All Users
- **Endpoint**: `GET /`
- **Query Parameters**:
    - `pageNumber`: integer (default 1)
    - `pageSize`: integer (default 10)
    - `search`: string (optional)
    - `sortBy`: string (default "name")
    - `sortDirection`: string ("asc" or "desc", default "asc")
- **Response Data**: `PagedResult<UserResponseDto>`.

### Get User By UID
- **Endpoint**: `GET /{uid}`
- **Response Data (`UserResponseDto`)**:
```json
{
  "id": integer,
  "uuid": "string",
  "name": "string",
  "email": "string",
  "bio": "string",
  "avatarUrl": "string",
  "status": boolean,
  "createdAt": "datetime",
  "updatedAt": "datetime",
  "roles": ["string"]
}
```

### Update User
- **Endpoint**: `PUT /{uid}`
- **Auth**: Admin only.
- **Request Body (`UserRequestDto`)**:
```json
{
  "name": "string",
  "bio": "string",
  "profilePictureUrl": "string"
}
```

---

## 3. Categories (`/api/Categories`)
*Authentication Required*

### Get All Categories
- **Endpoint**: `GET /`
- **Query Parameters**: Same as Users.
- **Response Data**: `PagedResult<CategoryResponseDto>`.

### Get Category By UID
- **Endpoint**: `GET /{uid}`
- **Response Data (`CategoryResponseDto`)**:
```json
{
  "id": integer,
  "uid": "guid",
  "slug": "string",
  "name": "string",
  "description": "string",
  "parentId": integer,
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### Get Category By Slug
- **Endpoint**: `GET /slug/{slug}`

### Create Category
- **Endpoint**: `POST /`
- **Auth**: Admin only.
- **Request Body (`CategoryRequestDto`)**:
```json
{
  "name": "string",
  "description": "string",
  "parentId": integer
}
```

### Update Category
- **Endpoint**: `PUT /{uid}`
- **Request Body**: Same as Create.

### Delete Category
- **Endpoint**: `DELETE /{uid}`
- **Auth**: Admin only.

---

## 4. Posts (`/api/Posts`)

### Get All Posts
- **Endpoint**: `GET /`
- **Query Parameters**: Same as Users (default `sortBy` is null, `sortDirection` is "desc").
- **Response Data**: `PagedResult<PostResponseDto>`.

### Get Post By UID
- **Endpoint**: `GET /{uid}`
- **Response Data (`PostResponseDto`)**:
```json
{
  "id": integer,
  "uid": "guid",
  "title": "string",
  "slug": "string",
  "excerpt": "string",
  "content": "string",
  "featuredImage": "string",
  "status": "string",
  "publishedAt": "datetime",
  "createdAt": "datetime",
  "updatedAt": "datetime",
  "author": {
    "id": integer,
    "name": "string",
    "email": "string"
  },
  "category": {
    "id": integer,
    "name": "string",
    "slug": "string"
  }
}
```

### Create Post
- **Endpoint**: `POST /`
- **Request Body (`CreatePostDto`)**:
```json
{
  "authorId": integer,
  "categoryId": integer,
  "title": "string",
  "slug": "string",
  "excerpt": "string",
  "content": "string",
  "featuredImage": "string",
  "status": "string",
  "publishedAt": "datetime"
}
```

---

## 5. Comments (`/api/Comments`)
*Authentication Required*

### Get Comment By UID
- **Endpoint**: `GET /{uid}`
- **Response Data (`CommentResponseDto`)**:
```json
{
  "id": integer,
  "uid": "string",
  "postId": integer,
  "userId": integer,
  "body": "string",
  "isApproved": boolean,
  "authorName": "string",
  "parentId": integer,
  "replies": [ /* Nested CommentResponseDto */ ],
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### Get Comments with Replies by Post ID
- **Endpoint**: `GET /post/{id}`
- **Response Data**: List of `CommentResponseDto` (with nested replies).

### Create Comment
- **Endpoint**: `POST /`
- **Request Body (`CreateCommentDto`)**:
```json
{
  "postId": integer,
  "userId": integer,
  "guestName": "string",
  "guestEmail": "string",
  "body": "string",
  "parentId": integer
}
```

### Update Comment
- **Endpoint**: `PUT /{uid}`
- **Request Body (`UpdateCommentDto`)**:
```json
{
  "body": "string"
}
```

---

## 6. Roles (`/api/Role`)
*Authentication Required*

### Get All Roles
- **Endpoint**: `GET /`
- **Query Parameters**: Same as Users (default `sortBy` is "name").
- **Response Data**: `PagedResult<RoleResponseDto>`.

### Get Role By UID
- **Endpoint**: `GET /{uid}`

### Create Role
- **Endpoint**: `POST /`
- **Auth**: Admin only.
- **Request Body (`RoleRequestDto`)**:
```json
{
  "name": "string",
  "description": "string"
}
```

### Update Role
- **Endpoint**: `PUT /{uid}`
- **Auth**: Admin only.
- **Request Body**: Same as Create.

---

## 7. Tags (`/api/Tags`)
*Authentication Required*

### Get All Tags
- **Endpoint**: `GET /`
- **Query Parameters**: Same as Users.
- **Response Data**: `PagedResult<TagResponseDto>`.

### Get Tag By UID
- **Endpoint**: `GET /{uid}`

### Get Tag By Slug
- **Endpoint**: `GET /slug/{slug}`

### Create Tag
- **Endpoint**: `POST /`
- **Auth**: Admin only.
- **Request Body (`TagRequestDto`)**:
```json
{
  "name": "string",
  "postId": integer
}
```

### Update Tag
- **Endpoint**: `PUT /{uid}`
- **Auth**: Admin only.

### Delete Tag
- **Endpoint**: `DELETE /{uid}`
- **Auth**: Admin only.
