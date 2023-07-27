-- Get all products of selected brand.
CREATE
OR REPLACE FUNCTION get_products_by_brand_id(search_id bigint)
RETURNS TABLE (
  id bigint,
  brand_name text,
  category_id bigint,
  name text,
  price money,
  quantity bigint,
  created_at timestamp)
  language plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT products.id,
       brands.name AS brand_name,
       products.category_id,
       products.name,
       products.price,
       products.quantity,
       products.created_at
FROM products
         INNER JOIN brands ON brands.id = products.brand_id
WHERE products.brand_id = search_id;
END;$$;

-- Select all brands with the number of their products
-- respectively. Order by the number of products.
CREATE
OR REPLACE FUNCTION get_brands_with_products_amount()
RETURNS TABLE (
  brand_id bigint,
  amount bigint)
  language plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT brands.id AS brand_id, COUNT(products.brand_id) AS amount
FROM brands
         LEFT JOIN products
                   ON brands.id = products.brand_id
GROUP BY brands.id
ORDER BY amount DESC;
END;$$;

-- Get all products for a given category and section.
CREATE
OR REPLACE FUNCTION get_products_by_section_and_category(search_section_id bigint, search_category_id bigint)
RETURNS TABLE (
  id bigint,
  brand_name text,
  category_name text,
  section_name text,
  name text,
  price money,
  quantity bigint,
  created_at timestamp)
  language plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT products.id,
       brands.name,
       categories.name,
       sections.name,
       products.name,
       products.price,
       products.quantity,
       products.created_at
FROM products
         INNER JOIN sections_categories ON products.category_id = sections_categories.category_id AND
                                           sections_categories.section_id = search_section_id
         INNER JOIN categories ON products.category_id = categories.id
         INNER JOIN brands ON products.brand_id = brands.id
         INNER JOIN sections ON sections_categories.section_id = sections.id
WHERE sections_categories.category_id = search_category_id;
END;$$;

-- Get all completed orders with a given product. Order from
-- newest to latest.
CREATE
OR REPLACE FUNCTION get_completed_orders_by_product_id(search_id bigint)
RETURNS TABLE (
  id bigint,
  user_id bigint,
  price money,
  address_id bigint,
  created_at timestamp,
  order_status order_status_type)
  language plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT orders.id, orders.user_id, orders.price, orders.address_id, orders.created_at, orders.order_status
FROM orders
         INNER JOIN orders_items ON orders.id = orders_items.order_id
WHERE orders.order_status = 'completed'
  AND orders_items.product_id = search_id
ORDER BY created_at DESC;
END;$$;

-- Get all reviews for a given product. Implement this as a view
-- table which contains rating, comment and info of a person
-- who left a comment.
CREATE
OR REPLACE VIEW get_all_reviews AS
SELECT reviews.raiting, reviews.title, reviews.comment, users.first_name, users.last_name, users.email
FROM reviews
         INNER JOIN users ON users.id = reviews.user_id;
