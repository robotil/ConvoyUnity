CC = g++

CFLAGS = -std=c++0x -fPIC -O -g

LDFLAGS = -shared

LIBS = -lboost_system -lboost_thread

TARGET = libvlpcommunication.so

OBJ_DIR = obj

INC_DIR = include

SRC_DIR = src

INCLUDES = -I$(INC_DIR)
# take all h files on include dir
DEPS = $(wildcard $(INC_DIR)/*.h)
# replace the name of all h files to the same name with o suffix
OBJ = $(DEPS:$(INC_DIR)/%.h=$(OBJ_DIR)/%.o)

all: directories $(TARGET)

# this rule meant to create obj directory if it does not exist already
directories: $(OBJ_DIR)

$(OBJ_DIR):
	mkdir -p $(OBJ_DIR)

$(OBJ_DIR)/%.o: $(SRC_DIR)/%.cpp $(DEPS)
	$(CC) $(CFLAGS) -c -o $@ $< $(INCLUDES)

$(TARGET): $(OBJ) 
	$(CC) $(LDFLAGS) -o $@ $^ $(LIBS)

.PHONY: all clean

clean:
	rm -f $(OBJ_DIR)/*.o *~ core
