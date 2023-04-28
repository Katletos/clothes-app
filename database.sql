CREATE TABLE Brands (
  brand_id bigserial,
  name text NOT NULL,
  PRIMARY KEY (brand_id)
);

CREATE TABLE Products (
  product_id bigserial,
  brand_id bigint NOT NULL,
  category_id bigint NOT NULL,
  price money NOT NULL,
  quantity bigint NOT NULL,
  created_at timestamp NOT NULL,
  PRIMARY KEY (product_id),
  FOREIGN KEY (brand_id) REFERENCES Brands (brand_id) ON DELETE CASCADE
);

CREATE TYPE user_type AS ENUM ('admin', 'customer');

CREATE TABLE Users (
  user_id bigserial,
  user_type user_type NOT NULL,
  email text NOT NULL,
  password text NOT NULL,
  phone text NOT NULL,
  first_name text NOT NULL,
  last_name text NOT NULL,
  created_at timestamp NOT NULL,
  PRIMARY KEY (user_id)
);

CREATE TABLE Addresses (
  address_id bigserial,
  user_id bigint NOT NULL,
  address text NOT NULL,
  PRIMARY KEY (address_id),
  FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE CASCADE
);

CREATE TYPE order_status_type AS ENUM ('inreview', 'indelivery', 'completed');

CREATE TABLE Orders (
  order_id bigserial,
  user_id bigint NOT NULL,
  price money NOT NULL,
  address_id bigint NOT NULL,
  created_at timestamp NOT NULL,
  order_status order_status_type NOT NULL,
  PRIMARY KEY (order_id),
  FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE RESTRICT,
  FOREIGN KEY (address_id) REFERENCES Addresses (address_id) ON DELETE RESTRICT
);

CREATE TABLE Orders_Items (
  product_id bigint,
  order_id bigint,
  quantity bigint NOT NULL,
  PRIMARY KEY (product_id, order_id),
  FOREIGN KEY (product_id) REFERENCES Products (product_id) ON DELETE RESTRICT,
  FOREIGN KEY (order_id) REFERENCES Orders (order_id) ON DELETE CASCADE
);

CREATE TABLE Orders_Transactions (
  order_transaction_id bigserial,
  order_id bigint NOT NULL,
  order_status order_status_type NOT NULL,
  updated_at timestamp NOT NULL,
  PRIMARY KEY (order_transaction_id),
  FOREIGN KEY (order_id) REFERENCES Orders (order_id) ON DELETE CASCADE
);

CREATE TABLE Media (
  image_id bigserial,
  product_id bigint NOT NULL,
  url text NOT NULL,
  file_type text NOT NULL,
  file_name text NOT NULL,
  PRIMARY KEY (image_id),
  FOREIGN KEY (product_id) REFERENCES Products (product_id) ON DELETE CASCADE
);

CREATE TABLE Categories (
  category_id bigserial,
  parent_category_id bigint,
  name text NOT NULL,
  PRIMARY KEY (category_id),
  FOREIGN KEY (parent_category_id) REFERENCES Categories (category_id)
);

CREATE TABLE Sections (
  section_id bigserial,
  name text NOT NULL,
  PRIMARY KEY (section_id)
);

CREATE TABLE Sections_Categories (
  category_id bigint,
  section_id bigint,
  PRIMARY KEY (category_id, section_id),
  FOREIGN KEY (category_id) REFERENCES Categories (category_id) ON DELETE CASCADE,
  FOREIGN KEY (section_id) REFERENCES Sections (section_id) ON DELETE CASCADE
);

CREATE TABLE Review (
  review_id bigserial,
  product_id bigint NOT NULL,
  user_id bigint NOT NULL,
  raiting smallint NOT NULL,
  title text NOT NULL,
  user_comment text NOT NULL,
  created_at timestamp NOT NULL,
  PRIMARY KEY (review_id),
  FOREIGN KEY (product_id) REFERENCES Products (product_id) ON DELETE CASCADE,
  FOREIGN KEY (user_id) REFERENCES Users (user_id)
);
