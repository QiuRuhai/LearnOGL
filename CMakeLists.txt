cmake_minimum_required(VERSION 3.10)
project(LearnOpenGL)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

include_directories(include)

find_package(OpenGL REQUIRED)

find_package(glfw3 REQUIRED)

file(GLOB SRC_FILES
    "src/main.cpp"
    "src/glad.c"
)

add_executable(${PROJECT_NAME} ${SRC_FILES}
        include/learnopengl/shader_s.h
        src/stb_image.cpp
        include/learnopengl/camera.h
)

target_link_libraries(${PROJECT_NAME}
    OpenGL::GL
    glfw
)


