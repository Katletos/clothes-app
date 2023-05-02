CREATE TABLE brands (
  id bigserial,
  name text NOT NULL UNIQUE,
  PRIMARY KEY (id)
);

CREATE TABLE products (
  id bigserial,
  brand_id bigint NOT NULL,
  category_id bigint NOT NULL,
  name text NOT NULL UNIQUE,
  price money NOT NULL,
  quantity bigint NOT NULL,
  created_at timestamp NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (brand_id) REFERENCES brands (id) ON DELETE CASCADE,
  FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE CASCADE
);

CREATE TYPE user_type AS ENUM ('admin', 'customer');

CREATE TABLE users (
  id bigserial,
  user_type user_type NOT NULL,
  email text NOT NULL UNIQUE,
  password text NOT NULL,
  phone text,
  first_name text,
  last_name text,
  created_at timestamp NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE addresses (
  id bigserial,
  user_id bigint NOT NULL,
  address text NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TYPE order_status_type AS ENUM ('inreview', 'indelivery', 'completed');

CREATE TABLE orders (
  id bigserial,
  user_id bigint NOT NULL,
  price money NOT NULL,
  address_id bigint NOT NULL,
  created_at timestamp NOT NULL,
  order_status order_status_type NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE RESTRICT,
  FOREIGN KEY (address_id) REFERENCES addresses (id) ON DELETE RESTRICT
);

CREATE TABLE orders_items (
  product_id bigint,
  order_id bigint,
  quantity bigint NOT NULL,
  price money NOT NULL,
  PRIMARY KEY (product_id, order_id),
  FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE RESTRICT,
  FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
);

CREATE TABLE orders_transactions (
  id bigserial,
  order_id bigint NOT NULL,
  order_status order_status_type NOT NULL,
  updated_at timestamp NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
);

CREATE TABLE media (
  id bigserial,
  product_id bigint NOT NULL,
  url text NOT NULL,
  file_type text NOT NULL,
  file_name text NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

CREATE TABLE categories (
  id bigserial,
  parent_category_id bigint,
  name text NOT NULL UNIQUE,
  PRIMARY KEY (id),
  FOREIGN KEY (parent_category_id) REFERENCES categories (id) ON DELETE CASCADE
);

CREATE TABLE sections (
  id bigserial,
  name text NOT NULL UNIQUE,
  PRIMARY KEY (id)
);
	
CREATE TABLE sections_categories (
  category_id bigint,
  section_id bigint,
  PRIMARY KEY (category_id, section_id),
  FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE CASCADE,
  FOREIGN KEY (section_id) REFERENCES sections (id) ON DELETE CASCADE
);

CREATE TABLE reviews (
  id bigserial,
  product_id bigint NOT NULL,
  user_id bigint NOT NULL,
  raiting smallint NOT NULL,
  title text NOT NULL,
  comment text NOT NULL,
  created_at timestamp NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE,
  FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE SET NULL 	
);
