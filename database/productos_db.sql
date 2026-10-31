CREATE DATABASE IF NOT EXISTS productos_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

USE productos_db;

CREATE TABLE IF NOT EXISTS productos (
    id             INT AUTO_INCREMENT PRIMARY KEY,
    nombre         VARCHAR(100)  NOT NULL,
    descripcion    VARCHAR(255)  NULL,
    precio         DECIMAL(10,2) NOT NULL,
    stock          INT           NOT NULL DEFAULT 0,
    fecha_creacion DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO productos (nombre, descripcion, precio, stock) VALUES
('Cafe moreno', 'Bolsa de 250 g', 85.00, 30),
('Leshe Shabo', 'Caja de 1 litro', 28.50, 60),
('Pan Bimbo', 'Bolsa de 500 g', 42.00, 20);
