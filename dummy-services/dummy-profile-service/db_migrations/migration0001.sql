\c profservice;
CREATE TABLE profiles (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    email TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
)