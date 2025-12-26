cmake_minimum_required(VERSION 3.20)

# 工程
project(LearnOpenGL LANGUAGES C CXX)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# vcpkg 提示（如果未设置工具链）
if(NOT DEFINED CMAKE_TOOLCHAIN_FILE)
    message(STATUS "提示: 使用 vcpkg 时请设置 CMAKE_TOOLCHAIN_FILE")
    message(STATUS "例如: cmake -DCMAKE_TOOLCHAIN_FILE=<vcpkg路径>/scripts/buildsystems/vcpkg.cmake ..")
endif()

# Apple Silicon: 强制 arm64（如你需要通用二进制可改为 "arm64;x86_64"）
if(APPLE)
    set(CMAKE_OSX_ARCHITECTURES "arm64" CACHE STRING "" FORCE)
    # 静默 OpenGL 弃用告警
    add_compile_definitions(GL_SILENCE_DEPRECATION=1)
endif()

# 源码/头文件
set(SRC
        src/main.cpp
        src/glad.c          # GLAD 的 C 源文件
        src/stb_image.cpp   # 你自己的 stb_image 实现文件
)

add_executable(${PROJECT_NAME} ${SRC})

# 你的头文件路径（含 learnopengl/、glad/、stb/ 等）
target_include_directories(${PROJECT_NAME} PRIVATE
        ${CMAKE_SOURCE_DIR}/include
)

# -------- OpenGL --------
find_package(OpenGL REQUIRED)
target_link_libraries(${PROJECT_NAME} PRIVATE OpenGL::GL)

# -------- GLFW --------
find_package(glfw3 CONFIG REQUIRED)
target_link_libraries(${PROJECT_NAME} PRIVATE glfw)

# -------- Assimp --------
find_package(assimp CONFIG REQUIRED)
target_link_libraries(${PROJECT_NAME} PRIVATE assimp::assimp)



