CC = g++

CFLAGS = -std=c++0x -fPIC -O -g

LDFLAGS = -shared

TARGET = libvlpcommunication.so

SRC_DIR = src

INCLUDES = -Iinclude/

LIBS = -lboost_system -lboost_thread

SRCS = $(wildcard $(SRC_DIR)/*.cpp)

OBJS = $(SRCS:$(SRC_DIR)/%.cpp=$(notdir)%.o)

all: $(TARGET)

# $@ and $^, are the left and right sides of the :
$(TARGET): $(OBJS)
	$(CC) $(LDFLAGS) -o $@ $^ $(LIBS)

$(notdir)%.o: $(SRC_DIR)/%.cpp
	$(CC) $(CFLAGS) $(INCLUDES) -c $(SRCS)

#. PHONY rule keeps make from doing something with a file named clean.
.PHONY: all clean
clean:
	$(RM) $(TARGET) ${OBJS}
