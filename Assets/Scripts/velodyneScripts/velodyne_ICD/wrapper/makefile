CC = g++

CFLAGS = -std=c++0x -fPIC -O -g

LDFLAGS = -shared

LIBS = libvlpcommunication.so

TARGET = libvlp.so

OBJ_DIR = obj

INC_DIR = include

SRC_DIR = src

INCLUDES = -I$(INC_DIR) -I../ICD/include/
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

#. PHONY rule keeps make from doing something with a file named clean.
.PHONY: all clean dirs

clean:
	rm -f $(OBJ_DIR)/*.o *~ core

