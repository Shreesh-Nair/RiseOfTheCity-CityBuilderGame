# CityBuilder

## Overview
CityBuilder is a casual, relaxing city-building simulation game where players construct and manage a growing city. It offers sandbox-style gameplay with simple progression mechanics, allowing players to design layouts, balance resources, and optimize growth while maintaining population morale.

---

## Unity Version
This project is developed using **Unity version 6000.0.32f1**.

---

## Features

### **Assets**
CityBuilder uses the [SimplePoly City - Low Poly Assets](https://assetstore.unity.com/packages/3d/environments/simplepoly-city-low-poly-assets-58899) pack from the Unity Asset Store. The key asset categories include:

#### **Buildings**
- Auto Service, Bakery, Bar, Bookshop, Chicken Shop, Clothing Store, Drug Store, Factory, Fast Food, Fruit Shop, Gas Station, Gift Shop, Music Store, Pizza Shop.
- Residential Buildings with color variations.
- Modern Houses (4 types) with 3 texture variations each.
- Sky Buildings (small and large), Stadium, Supermarket.

#### **Vehicles**
- 42 vehicle prefabs (21 with static wheels and 21 with separated wheels).
- Includes Trucks, Buses, Pickups, SUVs, Police Cars, Taxis, Ambulances.

#### **Nature**
- Trees (3 types), Bushes (3 types), Rocks (2 types), Grass fences, Pot bushes.

#### **Props**
- Benches (2 types), Billboards (3 types), Bus Stops, Coffee Shop Chairs, Dustbins.
- Roof Props: Antennas, Helipads, Solar Panels.
- Road Props: Streetlights, Traffic Cones, Control Barriers.

#### **Roads**
- Tile-based road system for easy modular construction.

---

## Game Description

### **Game Type**
CityBuilder is a single-player sandbox-style city-building simulation. Players start with a small area and limited building options. As they progress:
1. Unlock agricultural and industrial zones.
2. Expand land availability.
3. Gain access to new buildings and goals.

### **Core Gameplay Mechanics**
1. **Resource Management**: 
   - Agricultural zones produce food materials.
   - Industrial zones produce raw materials.
   - Players must balance these resources to meet population needs or trade excess resources to avoid deficits.

2. **Economy**:
   - Budget is managed through taxes: fixed income tax + GDP-based tax.
   - Buildings and services have maintenance costs deducted from the budget.

3. **Population Growth**:
   - Housing capacity determines population limits.
   - Immigration depends on morale, GDP growth rate, and housing availability.
   - Emigration occurs if morale drops too low.

4. **Morale System**:
   - Pollution and population density affect morale negatively.
   - Parks and greenery improve morale.
   - Service buildings like police stations and hospitals boost morale.

5. **Progression**:
   - Players earn XP by completing goals such as reaching population milestones or achieving GDP targets.
   - Leveling up unlocks new land plots and building types.

6. **Traffic Simulation**:
   - Vehicles move along roads using a waypoint system for visual effect.

---

## Goals & Progression

### **Main Objective**
Build a thriving city by:
1. Strategically placing buildings and roads.
2. Managing resources effectively.
3. Increasing population and income while maintaining high morale.

### **Progression System**
- Players start at Level 1 with limited land and basic structures.
- Completing goals grants XP that unlocks:
  1. New land plots for expansion.
  2. Agricultural and industrial zones.
  3. Advanced building types and challenges.

---

## Mechanics

### **Building Placement System**
- Grid-based placement ensures buildings fit predefined tiles.
- Buildings must be adjacent to roads to function properly.
- Some buildings depend on proximity to specific structures (e.g., factories near industrial zones).
- Placement costs vary based on building type and size.

### **Tile-Based Road System**
- Modular road construction allows players to connect their city piece by piece.
- Traffic flow mechanics simulate congestion in high-density areas.

### **Economy & Trade System**
- Income is generated through taxes (fixed income tax + GDP-based tax).
- Resources are produced in agricultural (food) and industrial (raw materials) zones:
  - Food/raw material shortages reduce morale.
  - Excess resources can be traded for balance.

### **Population Growth & Housing**
- Housing capacity determines the maximum population size.
- Immigration depends on morale, GDP growth rate, and housing availability.

### **Morale System**
Factors affecting morale include:
1. Pollution & Population Density: Parks help mitigate negative effects of high density.
2. Tax Rate: Higher taxes reduce morale; lower taxes decrease income.
3. Greenery & Decorations: Trees and parks improve morale significantly.
4. Service Buildings: Police stations, hospitals, schools boost citizen satisfaction.

### **XP & Level Progression**
Players gain XP by completing goals such as:
1. Reaching specific population milestones or GDP targets.
2. Constructing specific buildings or achieving resource balance goals.

Leveling up unlocks:
1. New land plots for expansion.
2. Additional building types (e.g., advanced factories or skyscrapers).
3. New challenges such as pollution control or trade optimization policies.

---

## Societal Benefits

CityBuilder offers several benefits for players:
1. Encourages creativity through city design and optimization challenges.
2. Introduces basic resource management skills in an engaging way.
3. Provides a relaxing environment for experimentation without pressure or violence.
4. Teaches logical thinking through planning and balancing city needs.

---

## Target Audience
CityBuilder is designed for:
1. All ages – Family-friendly gameplay with no violence or negative elements.
2. Casual gamers who enjoy sandbox-style simulation games.
3. Fans of city-building games seeking a simpler experience that is less time-consuming.

---

## Installation
1. Download Unity version 6000.0.32f1 or later from [Unity's official website](https://unity.com/).
2. Clone this repository to your local machine using Git or download the ZIP file directly from GitHub.
3. Import the [SimplePoly City asset pack](https://assetstore.unity.com/packages/3d/environments/simplepoly-city-low-poly-assets-58899) into your Unity project from the Unity Asset Store.

---

## License
This project uses assets from the Unity Asset Store under their respective licenses. Ensure compliance with usage terms when modifying or distributing the game.

---

Enjoy building your dream city! 😊
