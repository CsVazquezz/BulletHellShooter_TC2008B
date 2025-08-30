# Bullet Hell Shooter - TC2008B

Un juego de disparos espacial 2D tipo bullet hell desarrollado en Unity como parte del curso TC2008B de Programación de Gráficas. Presenta patrones de disparo matemáticos avanzados, object pooling y batallas de jefe basadas en fases.

## 🎮 Resumen del Juego

Este proyecto implementa un shooter bullet hell clásico donde los jugadores enfrentan un jefe desafiante con múltiples fases de ataque. El juego demuestra patrones matemáticos avanzados para la generación de balas, sistemas eficientes de object pooling y mecánicas de progresión basadas en tiempo.

### Características Principales

- **Batalla de Jefe Multi-Fase**: 3 fases distintas con dificultad creciente
- **Patrones de Disparo Matemáticos**: 
  - Patrones de disparo lineal
  - Formaciones radiales/espirales usando funciones matemáticas
  - Formaciones de patrones en estrella
- **Object Pooling Avanzado**: Sistema eficiente de manejo de balas
- **Progresión Basada en Tiempo**: Intervalos de fase de 10 segundos
- **Trayectorias Curvadas de Balas**: Cálculos dinámicos de rutas de balas

## 🛠️ Implementación Técnica

### Arquitectura

El juego usa una arquitectura modular con componentes especializados:

- **BossController**: Orquestador principal para el manejo de fases y coordinación de ataques
- **Sistemas de Disparo**: Implementaciones modulares de patrones de disparo
  - `LinearShoot.cs`: Patrones de balas en línea recta
  - `RadialShoot.cs`: Formaciones espirales y circulares usando funciones matemáticas
  - `StarShoot.cs`: Formaciones de patrones en estrella
- **Manejo de Balas**: 
  - `Bullet.cs`: Comportamiento individual de balas
  - `BulletPool.cs`: Sistema de object pooling para rendimiento
  - `BulletCurve.cs`: Cálculos avanzados de trayectoria
- **Utilidades**:
  - `TimeManager.cs`: Temporización del juego y manejo de fases
  - `BulletCounter.cs`: Monitoreo de rendimiento
  - `Turret.cs`: Componente base de disparo

### Patrones Matemáticos

El juego implementa patrones matemáticos sofisticados:

- **Patrones Espirales**: Usando la fórmula `r = a * e^(b*θ)` para generación de espirales
- **Distribuciones Radiales**: Formaciones circulares de balas con densidad ajustable
- **Formaciones Estrella**: Patrones de estrella multi-punto con brazos configurables

## 🎯 Fases del Juego

### Fase 1: Patrones Estrella (0-10 segundos)
- Múltiples ataques de formación en estrella
- Introducción de dificultad moderada

### Fase 2: Patrones Onda (10-20 segundos)
- Patrones de disparo lineal
- Densidad de balas incrementada

### Fase 3: Patrones Espiral (20+ segundos)
- Formaciones espirales matemáticas complejas
- Dificultad máxima con patrones espirales ajustados

## 🚀 Comenzando

### Prerequisitos

- Unity 6000.2.0f1 o superior
- Visual Studio o IDE de C# preferido

### Instalación

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/CsVazquezz/BulletHellShooter_TC2008B.git
   ```

2. Abrir el proyecto en Unity:
   - Iniciar Unity Hub
   - Hacer clic en "Abrir" y navegar a la carpeta `SpaceShooter/`
   - Abrir el proyecto

3. Abrir la escena principal:
   - Navegar a `Assets/Scenes/`
   - Abrir `SampleScene.unity`

4. Ejecutar el juego:
   - Presionar el botón Play en el Editor de Unity

### Controles

- **Movimiento**: Teclas de flecha o WASD
- **Disparo**: Barra espaciadora o entrada configurada
- **Pausa**: ESC (si está implementado)

## 🎨 Assets Utilizados

El proyecto incluye el pack de assets "Space Shooter Template FREE" que proporciona:
- Sprites de naves espaciales y animaciones
- Efectos visuales y sistemas de partículas
- Elementos de fondo y componentes de UI
- Efectos de audio

## 📦 Paquetes de Unity

Paquetes clave de Unity utilizados:
- **Input System** (1.14.1): Manejo moderno de entrada
- **Universal Render Pipeline** (17.2.0): Renderizado mejorado
- **2D Animation** (12.0.2): Animaciones de sprites
- **Visual Scripting** (1.9.7): Soporte de scripting basado en nodos

## 🔧 Optimización de Rendimiento

- **Object Pooling**: Previene picos de recolección de basura
- **Manejo Eficiente de Balas**: Reutiliza objetos de balas
- **Cálculos Matemáticos**: Generación optimizada de patrones
- **Arquitectura Modular**: Separación limpia de responsabilidades

## 📝 Contexto Académico

**Materia**: TC2008B - Programación de Gráficas  
**Matrícula**: A01711730  
**Institución**: Tecnológico de Monterrey

Este proyecto demuestra:
- Conceptos avanzados de programación matemática
- Mejores prácticas de desarrollo de juegos
- Patrones de diseño orientado a objetos
- Técnicas de optimización de rendimiento
- Programación de gráficas en tiempo real

## 🤝 Contribución

Este es un proyecto académico. Con fines educativos, siéntete libre de:
- Estudiar las implementaciones matemáticas
- Analizar el sistema de object pooling
- Examinar la arquitectura modular

## 📄 Licencia

Este proyecto está desarrollado con fines educativos como parte del currículo del curso TC2008B.

## 🎓 Resultados de Aprendizaje

A través de este proyecto, los conceptos clave dominados incluyen:
- Generación de patrones matemáticos en juegos
- Manejo eficiente de memoria con object pooling
- Sistemas de progresión de juego basados en tiempo
- Diseño de arquitectura modular de juegos
- Técnicas de optimización del motor Unity